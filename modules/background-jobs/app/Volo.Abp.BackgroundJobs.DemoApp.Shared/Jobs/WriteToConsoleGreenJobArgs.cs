namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    [BackgroundJobName("GreenJob")]
    public class WriteToConsoleGreenJobArgs
    {
        public string Value { get; set; }
    }
}