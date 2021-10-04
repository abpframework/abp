using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class AbpRabbitMqBackgroundJobOptions
    {
        /// <summary>
        /// Key: Job Args Type
        /// </summary>
        public Dictionary<Type, JobQueueConfiguration> JobQueues { get; }

        /// <summary>
        /// Default value: "AbpBackgroundJobs.".
        /// </summary>
        public string DefaultQueueNamePrefix { get; set; }

        /// <summary>
        /// Default value: "AbpBackgroundJobsDelayed."
        /// </summary>
        public string DefaultDelayedQueueNamePrefix { get; set;}

        public AbpRabbitMqBackgroundJobOptions()
        {
            JobQueues = new Dictionary<Type, JobQueueConfiguration>();
            DefaultQueueNamePrefix = "AbpBackgroundJobs.";
            DefaultDelayedQueueNamePrefix = "AbpBackgroundJobsDelayed.";
        }
    }
}
