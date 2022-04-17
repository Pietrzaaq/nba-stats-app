using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NbaStats.Domain.Entities;
using NbaStats.Worker.Models;
using Newtonsoft.Json;
using Quartz;
using Serilog;


namespace NbaStats.Worker.Jobs
{
    public class InitTeamsAndPlayersJob : IJob
    {
        private  List<Team> _teams;
        private  List<Player> _players;
        
        private IConfiguration configuration;
        public static HttpClient client;
        
        
        public InitTeamsAndPlayersJob()
        {
            _teams = new List<Team>();
            _players = new List<Player>();
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("InitTeamJob starting...");
            
            //Map the data from job detail
            JobDataMap dataMap = context.MergedJobDataMap;
            client = (HttpClient) dataMap["httpClient"];
            configuration = (IConfiguration) dataMap["configuration"];
            
            //Check if teams and players table is empty in the database
            using (var ctx = new NbaDatabaseContext())
            {
                if (ctx.Teams.Any() && ctx.Players.Any())
                {
                    Log.Information("Teams and players table are already initialized");
                    return;
                }
            }
            
            //Get teams list from api
            var url = $"{configuration.GetValue<string>("Api:ApiUrl:Teams")}{configuration.GetValue<string>("Api:ApiKey")}";
            
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonTeamsString = await response.Content.ReadAsStringAsync();
            
            //
            // string jsonTeamsString = "";
            //
            // using (StreamReader file = File.OpenText(@"./TeamsResponse.json"))
            // {
            //     jsonTeamsString = file.ReadToEnd();
            // }
            
            //Map the data from api response
            dynamic? jsonTeamList = JsonConvert.DeserializeObject(jsonTeamsString);

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
                    Log.Information("InitTeamsAndPlayersJob executed correctly");
                }
                catch (Exception e)
                {
                    Log.Error("Error occured while saving teams to database");
                    throw;
                }
            }
        }

        public async Task<bool> MapTeamsAndPlayersFromJson(dynamic jsonTeamList)
        {
            if (jsonTeamList is not null)
            {
                //Loop over every json team object
                foreach (var item in jsonTeamList)
                {
                    try
                    {
                        Team team = new Team
                        {
                            TeamId = item.TeamID,
                            IsApiId = true,
                            Key = item.Key,
                            Active = true,
                            City = item.City,
                            Name = item.Name,
                            LeagueId = item.LeagueID,
                            StadiumId = item.StadiumID,
                            Conference = item.Conference,
                            Division = item.Division,
                            PrimaryColor = item.PrimaryColor,
                            SecondaryColor = item.SecondaryColor,
                            TertiaryColor = item.TertiaryColor,
                            QuaternaryColor = item.QuaternaryColor,
                            WikipediaLogoUrl = item.WikipediaLogoUrl,
                            WikipediaWordMarkUrl = item.WikipediaWordMarkUrl,
                            UpdatedBy = "WorkerService"
                        };
                        
                        _teams.Add(team);
                        
                        //Get players list from api
                        var url = $"{configuration.GetValue<string>("Api:ApiUrl:Players")}{team.Key}{configuration.GetValue<string>("Api:ApiKey")}";
                        
                        var response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
            
                        var jsonPlayersString = await response.Content.ReadAsStringAsync();
                        
                        
                        // //Get the response for every team in loop and map it to the list of players
                        // string jsonPlayerString = "";
                        // using (StreamReader file = File.OpenText(@"./PlayerResponsePhi.json"))
                        // {
                        //     jsonPlayerString = file.ReadToEnd();
                        // }

                        dynamic? jsonPlayerList = JsonConvert.DeserializeObject(jsonPlayersString);
                        
                        MapPlayersFromJson(jsonPlayerList);
                        
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error occured while mapping json response to team object. Message: {0} ", e.Message);
                        throw;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MapPlayersFromJson(dynamic jsonPlayerList)
        {
            try
            {
                foreach (var item in jsonPlayerList)
                {
                    Player player = new Player
                    {
                        PlayerId = item.PlayerID,
                        IsApiId = true,
                        Status = item.Status,
                        TeamId = item.TeamID,
                        Team = item.Team,
                        Jersey = item.Jersey,
                        PositionCategory = item.PositionCategory,
                        Position = item.Position,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Height = item.Height,
                        Weight = item.Weight,
                        BirthDate = item.BirthDate,
                        BirthCity = item.BirthCity,
                        BirthState = item.BirthState,
                        BirthCountry = item.BirthCountry,
                        HighSchool = item.HighSchool,
                        College = item?.College,
                        Salary = item.Salary,
                        PhotoUrl = item.PhotoUrl,
                        Experience = item.Experience,
                        InjuryStatus = item.InjuryStatus,
                        InjuryBodyPart = item.InjuryBodyPart,
                        InjuryStartDate = item.InjuryStartDate,
                        InjuryNotes = item.InjuryNotes,
                        DepthChartPosition = item.DepthChartPosition,
                        DepthChartOrder = item.DepthChartOrder,
                        UsaTodayPlayerId = item.UsaTodayPlayerID,
                        UsaTodayHeadshotUrl = item.UsaTodayHeadshotUrl,
                        UsaTodayHeadshotNoBackgroundUrl = item.UsaTodayHeadshotNoBackgroundUrl,
                        UsaTodayHeadshotUpdated = item.UsaTodayHeadshotUpdated,
                        UsaTodayHeadshotNoBackgroundUpdated = item.UsaTodayHeadshotNoBackgroundUpdated,
                        NbaDotComPlayerId = item.NbaDotComPlayerID,
                        UpdatedBy = "WorkerService",
                        RefreshDate = DateTime.Now,
                    };
                    
                    _players.Add(player);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}