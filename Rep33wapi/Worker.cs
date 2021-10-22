using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rep33.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rep33wapi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                bool useTimer = AppSettings.GetAppSetting("use_timer").ToLower() == "true";
                if (useTimer)
                {
                    var launch_time = AppSettings.GetAppSetting("launch_time");
                    var curtime = DateTime.Now.ToString("HH:mm");
                    if (curtime == launch_time)
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        _logger.LogInformation($"useTimer: {useTimer}");
                    }
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
