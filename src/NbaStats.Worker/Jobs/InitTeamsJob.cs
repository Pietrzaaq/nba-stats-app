using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NbaStats.Worker.Models;
using Quartz;
using Serilog;

namespace NbaStats.Worker.Jobs
{
    public class InitTeamsJob : IJob
    {
        public static HttpClient client;
        public IConfiguration configuration;
        public DbManager dbManager;
        public NbaDatabaseContext ctx;
        public static int counter = 1;
        
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            client = (HttpClient) dataMap["httpClient"];
            
            Log.Information($"Executing the InitTeamsJob {counter} times");
            counter++;
            return Task.CompletedTask;
        }
    }
}