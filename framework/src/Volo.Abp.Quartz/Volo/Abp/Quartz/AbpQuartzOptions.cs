using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Quartz;

namespace Volo.Abp.Quartz
{
    public class AbpQuartzOptions
    {
        /// <summary>
        /// The quartz configuration. Available properties can be found within Quartz.Impl.StdSchedulerFactory.
        /// </summary>
        public NameValueCollection Properties { get; set; }

        public Action<IServiceCollectionQuartzConfigurator> Configurator { get; set; }

        /// <summary>
        /// How long Quartz should wait before starting. Default: 0.
        /// </summary>
        public TimeSpan StartDelay { get; set; }

        [NotNull]
        public Func<IScheduler, Task> StartSchedulerFactory
        {
            get => _startSchedulerFactory;
            set => _startSchedulerFactory = Check.NotNull(value, nameof(value));
        }
        private Func<IScheduler, Task> _startSchedulerFactory;

        public AbpQuartzOptions()
        {
            Properties = new NameValueCollection();
            StartDelay = new TimeSpan(0);
            _startSchedulerFactory = StartSchedulerAsync;
        }
        
        private async Task StartSchedulerAsync(IScheduler scheduler)
        {
            if (StartDelay.Ticks > 0)
            {
                await scheduler.StartDelayed(StartDelay);
            }
            else
            {
                await scheduler.Start();
            }
        }
    }
}
