namespace Volo.Abp.BackgroundJobs
{
    public interface IBackgroundJobExecuter
    {
        void Execute(JobExecutionContext context);
    }
}