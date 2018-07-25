using System;

namespace Volo.Abp.BackgroundJobs
{
    public class JobExecutionContext
    {
        public Type JobType { get; }

        public object JobArgs { get; }

        public JobExecutionResult Result { get; set; }

        public JobExecutionContext(Type jobType, object jobArgs)
        {
            JobType = jobType;
            JobArgs = jobArgs;
            Result = JobExecutionResult.Success;
        }
    }
}