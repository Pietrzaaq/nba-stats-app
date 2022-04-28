using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NbaStats.Domain.Entities;
using NbaStats.Worker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;

namespace NbaStats.Worker.Jobs
{
    sealed class InitGamesForSeasonJob
    {
        private List<Game> _games;
        
        // private IConfiguration configuration;
        // public static HttpClient client;
        public InitGamesForSeasonJob()
        {
            _games = new List<Game>();
        }

        public async Task<bool> Execute()
        {
            Log.Information("InitGamesForSeasonJob starting..."); 
            
            //Map the data from job detail
            // JobDataMap dataMap = context.MergedJobDataMap;
            // client = (HttpClient) dataMap["httpClient"];
            // configuration = (IConfiguration) dataMap["configuration"];
            
            using (var ctx = new NbaDatabaseContext())
            {
                //Check if there is no games in database and teams table is initialized
                if (ctx.Games.Any())
                {
                    Log.Information("Games table is already initialized");
                    return true;
                }
                if (!ctx.Teams.Any())
                {
                    Log.Information("Teams table is not initialized");
                    return true;
                }
            }
            
            //Get data from text file
            string jsonGamesString = "";
            
            using (StreamReader file = File.OpenText(@"./GamesResponse.json"))
            {
                jsonGamesString = file.ReadToEnd();
            }
            
            //Map the data from api response
            JArray jsonGamesList = (JArray) JsonConvert.DeserializeObject(jsonGamesString)!;
            
            //Check if the list has any values
            if(!jsonGamesList.HasValues) {return false;}
            
            //Start function which takes list of games and added it to the global list for every game
            bool executedSuccessfully  = MapGamesFromJson(jsonGamesList);

            //Use NbaDatabaseContext to save list of games to database
            if (executedSuccessfully)
            {
                try
                {
                    //Check if games table is empty
                    using (var ctx = new NbaDatabaseContext())
                    {
                        if (!ctx.Games.Any())
                        {
                            ctx.Games.AddRange(_games);
                            ctx.SaveChanges();

                            Log.Information("Games successfully saved to database");
                        }

                        else
                        {
                            Log.Information("Games table are already initialized");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error occured while saving games to database, message: {}", e.Message);
                    throw;
                }
                
                Log.Information("InitGamesJob executed correctly");
                return true;
            }
            else
            {
                Log.Error("InitGamesJob have finished with error");
                return false;
            }
        }

        public bool MapGamesFromJson(JArray jsonGamesList)
        {
            Log.Information($"Initializing {jsonGamesList.Count} games...");

            try
            {
                foreach (var item in jsonGamesList)
                {
                    Game game = new Game
                    {
                        GameId = item.Value<int>("GameID"),
                        IsApiId = true,
                        Season = item.Value<int?>("Season"),
                        SeasonType = item.Value<int?>("SeasonType"),
                        Status = item.Value<string?>("Status"),
                        Day = item.Value<DateTime?>("Day"),
                        DateTime = item.Value<DateTime?>("DateTime"),
                        AwayTeam = item.Value<string?>("AwayTeam"),
                        HomeTeam = item.Value<string?>("HomeTeam"),
                        AwayTeamId = item.Value<int>("AwayTeamID"),
                        HomeTeamId = item.Value<int>("HomeTeamID"),
                        StadiumId = item.Value<int?>("StadiumID"),
                        Channel = item.Value<string?>("Channel"),
                        AwayTeamScore = item.Value<int?>("AwayTeamScore"),
                        HomeTeamScore = item.Value<int?>("HomeTeamScore"),
                        Updated = item.Value<DateTime?>("Updated"),
                        PointSpread = item.Value<double?>("PointSpread"),
                        AwayTeamMoneyLine = item.Value<int?>("AwayTeamMoneyLine"),
                        HomeTeamMoneyLine = item.Value<int?>("HomeTeamMoneyLine"),
                        GlobalGameId = item.Value<int?>("GlobalGameID"),
                        GlobalAwayTeamId = item.Value<int?>("GlobalAwayTeamID"),
                        GlobalHomeTeamId = item.Value<int?>("GlobalHomeTeamID"),
                        PointSpreadAwayTeamMoneyLine = item.Value<int?>("PointSpreadAwayTeamMoneyLine"),
                        PointSpreadHomeTeamMoneyLine = item.Value<int?>("PointSpreadHomeTeamMoneyLine"),
                        LastPlay = item.Value<string?>("LastPlay"),
                        IsClosed = item.Value<bool?>("IsClosed"),
                        GameEndDateTime = item.Value<DateTime?>("GameEndDateTime"),
                        HomeRotationNumber = item.Value<int?>("HomeRotationNumber"),
                        AwayRotationNumber = item.Value<int?>("AwayRotationNumber"),
                        NeutralVenue = item.Value<bool?>("NeutralVenue"),
                        DateTimeUtc = item.Value<DateTime?>("DateTimeUTC"),
                        UpdatedBy = "WorkerService"
                    };

                    _games.Add(game);
                    
                }
            }
            catch (Exception e)
            {
                Log.Error("Error occured while mapping json response to game object. Message: {} ", e.Message);
                throw;
            }

            return _games.Any() && _games.Count == jsonGamesList.Count;
        }
    }
}