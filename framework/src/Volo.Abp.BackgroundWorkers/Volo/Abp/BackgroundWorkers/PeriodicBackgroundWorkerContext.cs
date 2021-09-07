using System;

namespace Volo.Abp.BackgroundWorkers
{
    public class PeriodicBackgroundWorkerContext
    {
        public IServiceProvider ServiceProvider { get; }

        public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}