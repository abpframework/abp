using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobOptions
    {
        public Dictionary<string, Type> JobTypes { get; }

        //TODO: Move options related to default backgroundjob manager to another class

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

        /// <summary>
        /// Default duration (as seconds) for the first wait on a failure.
        /// Default value: 60 (1 minutes).
        /// </summary>
        public int DefaultFirstWaitDuration { get; set; }

        /// <summary>
        /// Default timeout value (as seconds) for a job before it's abandoned (<see cref="BackgroundJobInfo.IsAbandoned"/>).
        /// Default value: 172,800 (2 days).
        /// </summary>
        public int DefaultTimeout { get; set; }

        /// <summary>
        /// Default wait factor for execution failures.
        /// This amount is multiplated by last wait time to calculate next wait time.
        /// Default value: 2.0.
        /// </summary>
        public double DefaultWaitFactor { get; set; }

        public BackgroundJobOptions()
        {
            JobTypes = new Dictionary<string, Type>();

            JobPollPeriod = 5000;
            DefaultFirstWaitDuration = 60;
            DefaultTimeout = 172800;
            DefaultWaitFactor = 2.0;
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