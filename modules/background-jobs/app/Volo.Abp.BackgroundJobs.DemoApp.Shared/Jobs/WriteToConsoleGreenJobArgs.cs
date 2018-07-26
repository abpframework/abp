using System;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    [BackgroundJobName("GreenJob")]
    public class WriteToConsoleGreenJobArgs
    {
        public string Value { get; set; }

        public DateTime Time { get; set; }

        public WriteToConsoleGreenJobArgs()
        {
            Time = DateTime.Now;
        }
    }
}