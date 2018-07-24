namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs
{
    [BackgroundJobName("YellowJob")]
    public class WriteToConsoleYellowJobArgs
    {
        public string Value { get; set; }
    }
}
