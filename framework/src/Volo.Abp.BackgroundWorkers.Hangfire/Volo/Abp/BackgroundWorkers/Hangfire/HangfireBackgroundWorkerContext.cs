using System;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public class HangfireBackgroundWorkerContext : PeriodicBackgroundWorkerContext
    {
        public object Options { get; set; }

        public HangfireBackgroundWorkerContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}