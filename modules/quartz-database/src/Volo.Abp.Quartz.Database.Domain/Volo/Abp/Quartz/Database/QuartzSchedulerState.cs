using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_SCHEDULER_STATE
    /// </summary>
    public class QuartzSchedulerState : Entity
    {
        public string SchedulerName { get; set; }

        public string InstanceName { get; set; }

        public long LastCheckInTime { get; set; }

        public long CheckInInterval { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { SchedulerName, InstanceName };
        }
    }
}
