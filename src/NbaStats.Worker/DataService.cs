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
using Quartz.Impl.Triggers;

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
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://github.com/Pietrzaaq/nba-stats-app");
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
            
            return Task.CompletedTask;
        }

        public void StartScheduler()
        {
            _scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();

            ScheduleExampleJob();
            ScheduleInitTeamsJob();
        }
        
        public void ScheduleExampleJob()
        {
            _logger.LogInformation("Starting ExampleJob...");
            
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

            // Tell quartz to schedule the job using our trigger
            _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void ScheduleInitTeamsJob()
        {
            _logger.LogInformation("Starting InitTeamsJob");
            
            var jobData = new JobDataMap();
            jobData.Put("httpClient", _client);
            jobData.Put("configuration", _configuration);

            IJobDetail job = JobBuilder.Create<InitTeamsJob>()
                .UsingJobData(jobData)
                .WithIdentity("InitTeamsJob", "InitGroup")
                .Build();
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity("InitTeamsTrigger", "InitGroup")
                .WithSimpleSchedule(x => x
                    .WithRepeatCount(0)
                    .WithIntervalInSeconds(120)
                )
                .StartAt(DateTimeOffset.Now.AddMinutes(1))
                .Build();
            
            _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
        public void StopScheduler()
        {
            _logger.LogInformation("Stopping scheduler...");
            _scheduler.Shutdown().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}