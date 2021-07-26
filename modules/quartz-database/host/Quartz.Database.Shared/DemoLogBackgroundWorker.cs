using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace QuartzDatabaseDemo
{
    [DisallowConcurrentExecution]
    public class DemoLogBackgroundWorker : QuartzBackgroundWorkerBase
    {
        public DemoLogBackgroundWorker()
        {
            JobDetail = JobBuilder.Create<DemoLogBackgroundWorker>().WithIdentity(nameof(DemoLogBackgroundWorker)).Build();
            Trigger = TriggerBuilder.Create()
                .WithIdentity(nameof(DemoLogBackgroundWorker))
                .WithCronSchedule("0/5 * * * * ?")
                .Build();
        }

        public override Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("Executed DemoLogBackgroundWorker..!");
            return Task.CompletedTask;
        }
    }
}
