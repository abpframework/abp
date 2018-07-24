namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs
{
    [BackgroundJobName("GreenJob")]
    public class WriteToConsoleGreenJobArgs
    {
        public string Value { get; set; }
    }
}