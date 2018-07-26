using System;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    [BackgroundJobName("YellowJob")]
    public class WriteToConsoleYellowJobArgs
    {
        public string Value { get; set; }

        public DateTime Time { get; set; }

        public WriteToConsoleYellowJobArgs()
        {
            Time = DateTime.Now;
        }
    }
}
