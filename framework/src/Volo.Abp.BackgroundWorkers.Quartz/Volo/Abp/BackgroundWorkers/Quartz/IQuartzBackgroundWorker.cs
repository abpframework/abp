using System;
using System.Threading.Tasks;
using Quartz;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    public interface IQuartzBackgroundWorker : IBackgroundWorker, IJob
    {
        ITrigger Trigger { get; set; }

        IJobDetail JobDetail { get; set; }
        
        bool AutoRegister { get; set; }

        Func<IScheduler,Task> ScheduleJob { get; set; }
    }
}
