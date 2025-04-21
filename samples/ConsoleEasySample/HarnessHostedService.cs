using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleSample
{
    public class HarnessHostedService : BackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly ILogger<HarnessHostedService> _logger;

        public HarnessHostedService(IBackgroundJobClient backgroundJobs, ILogger<HarnessHostedService> logger)
        {
            _backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            TestMethod tm = new TestMethod();
            //tm.Age = 14;
            BackgroundJob.Schedule(() => tm.Output("123"), TimeSpan.FromSeconds(1));
            BackgroundJob.Schedule(() => tm.Output("456"), TimeSpan.FromSeconds(1));
            //BackgroundJob.Schedule<TestMethod>(x => x.Output("123"), TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public static void Empty()
        {
        }
    }
}