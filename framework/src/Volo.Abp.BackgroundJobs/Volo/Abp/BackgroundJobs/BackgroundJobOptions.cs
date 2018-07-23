namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobOptions
    {
        public bool IsJobExecutionEnabled { get; set; }
        
        public BackgroundJobOptions()
        {
            IsJobExecutionEnabled = true;
        }
    }
}