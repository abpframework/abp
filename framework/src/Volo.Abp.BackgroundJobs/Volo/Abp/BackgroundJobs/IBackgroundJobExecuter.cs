namespace Volo.Abp.BackgroundJobs
{
    public interface IBackgroundJobExecuter
    {
        void Execute(BackgroundJobInfo jobInfo);
    }
}