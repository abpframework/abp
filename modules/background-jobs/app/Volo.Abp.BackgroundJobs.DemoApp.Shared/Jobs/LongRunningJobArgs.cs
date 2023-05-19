namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    [BackgroundJobName("LongJob")]
    public class LongRunningJobArgs
    {
        public string Value { get; set; }
    }
}