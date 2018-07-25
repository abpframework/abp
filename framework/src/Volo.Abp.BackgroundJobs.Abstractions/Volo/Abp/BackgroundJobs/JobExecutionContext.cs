using System;

namespace Volo.Abp.BackgroundJobs
{
    public class JobExecutionContext
    {
        public Type JobType { get; }

        public object JobArgs { get; }

        public JobExecutionContext(Type jobType, object jobArgs)
        {
            JobType = jobType;
            JobArgs = jobArgs;
        }
    }
}