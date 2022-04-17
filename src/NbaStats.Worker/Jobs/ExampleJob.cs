using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NbaStats.Domain.Entities;
using NbaStats.Worker.Models;
using Newtonsoft.Json;
using Quartz;
using Serilog;


namespace NbaStats.Worker.Jobs
{
    public class ExampleJob: IJob
    {
        private List<Team> teams;
        public static HttpClient client;
        public IConfiguration configuration;
        public DbManager dbManager;
        public static int counter = 1;

        public ExampleJob()
        {
            teams = new List<Team>();
        }
        public Task Execute(IJobExecutionContext context)
        {
            //Map the data from job detail
            JobDataMap dataMap = context.MergedJobDataMap;
            client = (HttpClient) dataMap["httpClient"];
            configuration = (IConfiguration) dataMap["configuration"];


            Log.Information($"Executing the HelloJob {counter} times");
            counter++;
            return Task.CompletedTask;
        }
    }
}