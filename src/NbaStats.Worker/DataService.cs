using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NbaStats.Worker.Jobs;
using Quartz;
using Quartz.Impl;

namespace NbaStats.Worker
{
    public class DataService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataService> _logger;
        private readonly IScheduler _scheduler;
        private static HttpClient _client;
        
        public DataService(IConfiguration configuration, ILogger<DataService> logger)
        {
            //Get services and configure scheduler
            _configuration = configuration;
            _logger = logger;
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
            
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopScheduler();
            _client.Dispose();
            return base.StopAsync(cancellationToken);
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting the Quartz Scheduler");
            
            try
            {
                StartScheduler();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error occured while starting the Quartz Scheduler");
                throw;
            }

            var currentJobs = _scheduler.GetCurrentlyExecutingJobs().Result;
            _scheduler.GetTrigger(new TriggerKey("InitTeamsAndPlayersJob"));
            _scheduler.GetTrigger(new TriggerKey("InitTeamsTrigger"));
            
            return Task.CompletedTask;
        }

        public void StartScheduler()
        {
            _scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();

            //Schedule jobs
            ScheduleExampleJob();
            ScheduleInitTeamsAndPlayersJob();
            ScheduleInitGamesJob();
        }
        
        public void StopScheduler()
        {
            _logger.LogInformation("Stopping scheduler...");
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
            _logger.LogInformation("ExampleJob has been scheduled");
        }

        public void ScheduleInitTeamsAndPlayersJob()
        {
            var jobData = new JobDataMap();
            jobData.Put("httpClient", _client);
            jobData.Put("configuration", _configuration);

            IJobDetail job = JobBuilder.Create<InitTeamsAndPlayersJob>()
                .UsingJobData(jobData)
                .WithIdentity("InitTeamsAndPlayersJob", "InitGroup")
                .Build();
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity("InitTeamsTrigger", "InitGroup")
                .WithSimpleSchedule(x => x
                    .WithRepeatCount(0)
                    .WithIntervalInSeconds(10)
                )
                .StartAt(DateTimeOffset.Now.AddSeconds(10))
                .Build();
            
            _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
            _logger.LogInformation("InitTeamsAndPlayersJob has been scheduled");
        }
        
        public void ScheduleInitGamesJob()
        {
            //Use JobDataMap function to pass object to Job instances
            var jobData = new JobDataMap();
            jobData.Put("httpClient", _client);
            jobData.Put("configuration", _configuration);

            //Build job and assign it to the group
            IJobDetail job = JobBuilder.Create<InitGamesForSeasonJob>()
                .UsingJobData(jobData)
                .WithIdentity("InitGamesJob", "InitGroup")
                .Build();
            
            //Create trigger for the job and set interval 
            var trigger = TriggerBuilder.Create()
                .WithIdentity("InitGamesTrigger", "InitGroup")
                .WithSimpleSchedule(x => x
                    .WithRepeatCount(0)
                    .WithIntervalInSeconds(10)
                )
                .StartAt(DateTimeOffset.Now.AddSeconds(30))
                .Build();
            
            _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
            _logger.LogInformation("InitTeamsAndPlayersJob has been scheduled");
        }

    }
}