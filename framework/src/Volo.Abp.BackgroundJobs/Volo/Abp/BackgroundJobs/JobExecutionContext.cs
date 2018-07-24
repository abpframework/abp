namespace Volo.Abp.BackgroundJobs
{
    public class JobExecutionContext
    {
        public string JobName { get; }

        public string JobArgs { get; }

        public JobExecutionResult Result { get; set; }

        public JobExecutionContext(string jobName, string jobArgs)
        {
            JobName = jobName;
            JobArgs = jobArgs;
            Result = JobExecutionResult.Success;
        }
    }
}