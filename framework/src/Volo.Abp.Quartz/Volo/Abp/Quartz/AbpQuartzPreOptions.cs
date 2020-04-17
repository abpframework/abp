using System;
using System.Collections.Specialized;

namespace Volo.Abp.Quartz
{
    public class AbpQuartzPreOptions
    {
        /// <summary>
        /// The quartz configuration. Available properties can be found within Quartz.Impl.StdSchedulerFactory.
        /// </summary>
        public NameValueCollection Properties { get; set; }
        /// <summary>
        /// How long Quartz should wait before starting. Default: 0.
        /// </summary>
        public TimeSpan StartDelay { get; set; }

        public AbpQuartzPreOptions()
        {
            Properties = new NameValueCollection();
            StartDelay = new TimeSpan(0);
        }
    }
}
