using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NbaStats.Domain.Entities;
using NbaStats.Worker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;


namespace NbaStats.Worker.Jobs
{
    sealed class InitTeamsAndPlayersJob 
    {
        private  List<Team> _teams;
        private  List<Player> _players;
        
        private IConfiguration configuration;
        public static HttpClient client;
        
        // private readonly float _inchesToCentimeterMultiplier = 2.54f;
        // private readonly float _poundsToKilogramsMultiplier = 0.45359237f;
        
        public InitTeamsAndPlayersJob()
        {
            _teams = new List<Team>();
            _players = new List<Player>();
        }
        
        public async Task<bool> Execute()
        {
            Log.Information("InitTeamJob starting...");
            
            // //Map the data from job detail
            // JobDataMap dataMap = context.MergedJobDataMap;
            // client = (HttpClient) dataMap["httpClient"];
            // configuration = (IConfiguration) dataMap["configuration"];
            
            //Check if teams and players table is empty in the database
            using (var ctx = new NbaDatabaseContext())
            {
                if (ctx.Teams.Any() && ctx.Players.Any())
                {
                    Log.Information("Teams and players table are already initialized");
                    return true;
                }
            }
            
            //Get teams list from api
            var url = $"{configuration.GetValue<string>("Api:ApiUrl:Teams")}{configuration.GetValue<string>("Api:ApiKey")}";
            
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonTeamsString = await response.Content.ReadAsStringAsync();
            
            //For tests
            // string jsonTeamsString = "";
            //
            // using (StreamReader file = File.OpenText(@"./TeamsResponse.json"))
            // {
            //     jsonTeamsString = file.ReadToEnd();
            // }
            
            //Map the data from api response
            JArray jsonTeamList = (JArray) JsonConvert.DeserializeObject(jsonTeamsString)!;
            
            //Check if the list has any values
            if(!jsonTeamList.HasValues) {return false;}

            // var first = jsonTeamList.First;
            // var last = jsonTeamList.Last;
            // var highestWeight = jsonTeamList.Select(x => x.Value<int>("TeamID")).Max();
            // var lowestWeight = jsonTeamList.Select(x => x.Value<int>("TeamID")).Min();
            
            //Start function which takes list of players and added it to the global list for every team
            Task<bool> task = MapTeamsAndPlayersFromJson(jsonTeamList);
            bool executedSuccessfully = task.GetAwaiter().GetResult();
            
            //Use NbaDatabaseContext to save list of teams to database
            if (executedSuccessfully)
            {
                try
                {
                    using (var ctx = new NbaDatabaseContext())
                    {
                        if (!ctx.Teams.Any() && !ctx.Players.Any())
                        {
                            ctx.Teams.AddRange(_teams);
                            ctx.SaveChanges();
                            Log.Information($"Teams successfully saved to database");
                        
                            ctx.Players.AddRange(_players);
                            ctx.SaveChanges();
                            Log.Information($"Players successfully saved to database");
                        }
                        else
                        {
                            Log.Information("Teams and players table are already initialized");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error occured while saving teams to database, message: {}" , e.Message);
                    throw;
                }
                Log.Information("InitTeamsAndPlayersJob executed correctly");
                return true;
            }
            else
            {
                Log.Error("InitTeamsAndPlayersJob have finished with error");
                return false;
            }
        }

        public async Task<bool> MapTeamsAndPlayersFromJson(JArray jsonTeamList)
        {
            //Loop over every json team object
            foreach (var item in jsonTeamList)
            {
                try
                {
                    Team team = new Team
                    {
                        TeamId = item.Value<int>("TeamID"),
                        IsApiId = true,
                        Key = item.Value<string>("Key"),
                        Active = true,
                        City = item.Value<string>("City"),
                        Name = item.Value<string>("Name"),
                        LeagueId = item.Value<int>("LeagueID"),
                        StadiumId = item.Value<int>("StadiumID"),
                        Conference = item.Value<string>("Conference"),
                        Division = item.Value<string>("Division"),
                        PrimaryColor = item.Value<string>("PrimaryColor"),
                        SecondaryColor = item.Value<string>("SecondaryColor"),
                        TertiaryColor = item.Value<string>("TertiaryColor"),
                        QuaternaryColor = item.Value<string>("QuaternaryColor"),
                        WikipediaLogoUrl = item.Value<string>("WikipediaLogoUrl"),
                        WikipediaWordMarkUrl = item.Value<string>("WikipediaWordMarkUrl"),
                        UpdatedBy = "WorkerService"
                    };
                    
                    _teams.Add(team);
                    
                    //Get players list from api
                    var url = $"{configuration.GetValue<string>("Api:ApiUrl:Players")}{team.Key}{configuration.GetValue<string>("Api:ApiKey")}";
                    
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    
                    var jsonPlayersString = await response.Content.ReadAsStringAsync();
                    
                    
                    // //Get the response for every team in loop and map it to the list of players
                    // string jsonPlayersString = "";
                    // using (StreamReader file = File.OpenText(@"./PlayerResponseWiz.json"))
                    // {
                    //     jsonPlayersString = file.ReadToEnd();
                    // }

                    JArray jsonPlayerList = (JArray) JsonConvert.DeserializeObject(jsonPlayersString)!;
                    
                    MapPlayersFromJson(jsonPlayerList);
                    
                }
                catch (Exception e)
                {
                    Log.Error("Error occured while mapping json response to team object. Message: {} ", e.Message);
                    throw;
                }
            }

            return _teams.Any() && _players.Any() && _teams.Count == jsonTeamList.Count;
        }

        public void MapPlayersFromJson(JArray jsonPlayerList)
        {
            try
            {
                foreach (var item in jsonPlayerList)
                {

                    Player player = new Player
                    {
                        PlayerId = item.Value<int>("PlayerID"),
                        IsApiId = true,
                        Status = item.Value<string?>("Status"),
                        TeamId = item.Value<int?>("TeamID"),
                        Team = item.Value<string?>("Team"),
                        Jersey = item.Value<int?>("Jersey"),
                        PositionCategory = item.Value<string?>("PositionCategory"),
                        Position = item.Value<string?>("Position"),
                        FirstName = item.Value<string?>("FirstName"),
                        LastName = item.Value<string?>("LastName"),
                        Height = item.Value<int?>("Height"),
                        Weight = item.Value<int?>("Weight"),
                        BirthDate = item.Value<DateTime?>("BirthDate"),
                        BirthCity = item.Value<string?>("BirthCity"),
                        BirthState = item.Value<string?>("BirthState"),
                        BirthCountry = item.Value<string?>("BirthCountry"),
                        HighSchool = item.Value<string?>("HighSchool"),
                        College = item.Value<string?>("College"),
                        Salary = item.Value<int?>("Salary"),
                        PhotoUrl = item.Value<string?>("PhotoUrl"),
                        Experience = item.Value<int?>("Experience"),
                        InjuryStatus = item.Value<string?>("InjuryStatus"),
                        InjuryBodyPart = item.Value<string?>("InjuryBodyPart"),
                        InjuryStartDate = item.Value<DateTime?>("InjuryStartDate"),
                        InjuryNotes = item.Value<string?>("InjuryNotes"),
                        DepthChartPosition = item.Value<string?>("DepthChartPosition"),
                        DepthChartOrder = item.Value<int?>("DepthChartOrder"),
                        UsaTodayPlayerId = item.Value<int?>("UsaTodayPlayerID"),
                        UsaTodayHeadshotUrl = item.Value<string?>("UsaTodayHeadshotUrl"),
                        UsaTodayHeadshotNoBackgroundUrl = item.Value<string?>("UsaTodayHeadshotNoBackgroundUrl"),
                        UsaTodayHeadshotUpdated = item.Value<DateTime?>("UsaTodayHeadshotUpdated"),
                        UsaTodayHeadshotNoBackgroundUpdated = item.Value<DateTime?>("UsaTodayHeadshotNoBackgroundUpdated"),
                        NbaDotComPlayerId = item.Value<int?>("NbaDotComPlayerID"),
                        UpdatedBy = "WorkerService",
                        RefreshDate = DateTime.Now,
                    };
                    
                    _players.Add(player);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error occured while mapping json response to player object. Message: {} ", e.Message);
                throw;
            }
        }
    }
}