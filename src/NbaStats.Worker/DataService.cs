using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NbaStats.Worker.Jobs;
using NbaStats.Worker.Models;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace NbaStats.Worker
{
    public class DataService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IScheduler _scheduler;
        private static HttpClient _client;
        private static bool _tablesInitialized;
        
        public DataService(IConfiguration configuration, ILogger<DataService> logger)
        {
            //Get services and configure scheduler
            _configuration = configuration;
            _tablesInitialized = false;
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
                { "quartz.scheduler.instanceName", "StatsScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" }
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            _scheduler = factory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Initialize static instance of HttpClient
            _client = new HttpClient();
            
            //Run the function which initialize tables or check if its already initialized
            try
            {
                _tablesInitialized = InitializeTables();
                if (!_tablesInitialized)
                {
                    return StopAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Log.Error(e,"Error occured while initializing tables");
                throw;
            }
            
            //Run the function which initializes scheduler
            try
            {
                StartScheduler();
            }
            catch (Exception e)
            {
                Log.Error(e,"Error occured while initializing scheduler");
                throw;
            }
            
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopScheduler();
            _client.Dispose();
            return base.StopAsync(cancellationToken);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Quartz Scheduler is running");
                
                try
                {
                    await Task.Delay(5000);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        public bool InitializeTables()
        {
            bool isTeamsAndPlayersInitialized = new InitTeamsAndPlayersJob().Execute().Result;

            bool isGamesInitialized = new InitGamesForSeasonJob().Execute().Result;

            return isTeamsAndPlayersInitialized && isGamesInitialized;
        }
        

        public void StartScheduler()
        {
            _scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();
            
            ScheduleExampleJob();
        }
        
        public void StopScheduler()
        {
            Log.Information("Stopping scheduler...");
            _scheduler.Shutdown().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
        public void ScheduleExampleJob()
        {
            var jobData = new JobDataMap();
            jobData.Put("httpClient", _client);
            jobData.Put("configuration", _configuration);
            
            IJobDetail job = JobBuilder.Create<ExampleJob>()
                .UsingJobData(jobData)
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();
            
            _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
            Log.Information("ExampleJob has been scheduled");
        }

        // public void ScheduleInitTeamsAndPlayersJob()
        // {
        //     var jobData = new JobDataMap();
        //     jobData.Put("httpClient", _client);
        //     jobData.Put("configuration", _configuration);
        //
        //     _teamInitJobDetail = JobBuilder.Create<InitTeamsAndPlayersJob>()
        //         .UsingJobData(jobData)
        //         .WithIdentity("InitTeamsAndPlayersJob", "InitGroup")
        //         .Build();
        //
        //     // var trigger = TriggerBuilder.Create()
        //     //     .WithIdentity("InitTeamsTrigger", "InitGroup")
        //     //     .WithSimpleSchedule(x => x
        //     //         .WithRepeatCount(0)
        //     //         .WithIntervalInSeconds(10)
        //     //     )
        //     //     .StartAt(DateTimeOffset.Now.AddSeconds(10))
        //     //     .Build();
        //     //
        //     // _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
        //     Log.Information("InitTeamsAndPlayersJob has been scheduled");
        // }
        
        // public void ScheduleInitGamesJob()
        // {
        //     //Use JobDataMap function to pass object to Job instances
        //     var jobData = new JobDataMap();
        //     jobData.Put("httpClient", _client);
        //     jobData.Put("configuration", _configuration);
        //
        //     //Build job and assign it to the group
        //     IJobDetail job = JobBuilder.Create<InitGamesForSeasonJob>()
        //         .UsingJobData(jobData)
        //         .WithIdentity("InitGamesJob", "InitGroup")
        //         .Build();
        //
        //     // //Create trigger for the job and set interval 
        //     // var trigger = TriggerBuilder.Create()
        //     //     .WithIdentity("InitGamesTrigger", "InitGroup")
        //     //     .WithSimpleSchedule(x => x
        //     //         .WithRepeatCount(0)
        //     //         .WithIntervalInSeconds(10)
        //     //     )
        //     //     .StartAt(DateTimeOffset.Now.AddSeconds(30))
        //     //     .Build();
        //     //
        //     // _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
        //     
        //     Log.Information("InitTeamsAndPlayersJob has been prepared");
        // }

    }
}