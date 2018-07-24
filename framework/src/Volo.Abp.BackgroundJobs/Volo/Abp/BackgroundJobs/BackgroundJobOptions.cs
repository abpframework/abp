using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs
{
    //TODO: Move options related to default backgroundjob manager to another class
    //TODO: Create Volo.Abp.BackgroundJobs.Abstractions package and move abstractions there!

    public class BackgroundJobOptions
    {
        public Dictionary<string, Type> JobTypes { get; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsJobExecutionEnabled { get; set; } = true;

        /// <summary>
        /// Interval between polling jobs from <see cref="IBackgroundJobStore"/>.
        /// Default value: 5000 (5 seconds).
        /// </summary>
        public int JobPollPeriod { get; set; }

        /// <summary>
        /// Maximum count of jobs to fetch from data store in one loop.
        /// </summary>
        public int MaxJobFetchCount { get; set; } = 1000;

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