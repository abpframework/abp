using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobOptions
    {
        public Dictionary<string, Type> JobTypes { get; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public BackgroundJobOptions()
        {
            JobTypes = new Dictionary<string, Type>();
        }

        internal Type GetJobType(string jobName)
        {
            var jobType = JobTypes.GetOrDefault(jobName);

            if (jobType == null)
            {
                throw new AbpException("Undefined background job type for the job name: " + jobName);
            }

            return jobType;
        }
    }
}