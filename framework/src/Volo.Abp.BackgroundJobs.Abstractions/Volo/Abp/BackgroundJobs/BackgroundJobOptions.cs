using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobOptions
    {
        public Dictionary<string, Type> JobTypes { get; }

        //TODO: Implement for all providers! (Hangfire does not implement yet)
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsJobExecutionEnabled { get; set; } = true;

        public BackgroundJobOptions()
        {
            JobTypes = new Dictionary<string, Type>();
        }

        public Type GetJobType(string jobName)
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