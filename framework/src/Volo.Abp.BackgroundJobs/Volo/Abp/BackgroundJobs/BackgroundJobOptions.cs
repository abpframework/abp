using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobOptions
    {
        public Dictionary<string, Type> JobTypes { get; }

        public bool IsJobExecutionEnabled { get; set; }
        
        public BackgroundJobOptions()
        {
            IsJobExecutionEnabled = true;
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