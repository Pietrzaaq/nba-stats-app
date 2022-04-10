using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NbaStats.Worker.Jobs;
using Quartz;
using Quartz.Impl;

namespace NbaStats.Worker
{
    public class DataService : BackgroundService
    {
        private readonly ILogger<DataService> _logger;
        private readonly IScheduler _scheduler;
        private HttpClient _client;

        public DataService(ILogger<DataService> logger)
        {
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
                _logger.LogError(e,"Error occured while starting the _scheduler");
                throw;
            }
            
            return Task.CompletedTask;
        }

        public void StartScheduler()
        {
            _scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();

            ScheduleJobs();
        }
        
        public void ScheduleJobs()
        {
            var jobData = new JobDataMap();
            jobData.Put("httpClient", _client);
            
            IJobDetail job = JobBuilder.Create<ExampleJob>()
                .UsingJobData(jobData)
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
        public void StopScheduler()
        {
            _logger.LogInformation("Stopping scheduler...");
            _scheduler.Shutdown().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}