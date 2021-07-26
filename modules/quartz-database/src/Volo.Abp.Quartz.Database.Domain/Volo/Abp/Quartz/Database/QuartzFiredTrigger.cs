using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_FIRED_TRIGGERS
    /// </summary>
    public class QuartzFiredTrigger : Entity
    {
        public string SchedulerName { get; set; }

        public string EntryId { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public string InstanceName { get; set; }

        public long FiredTime { get; set; }

        public long ScheduledTime { get; set; }

        public int Priority { get; set; }

        public string State { get; set; }

        public string JobName { get; set; }

        public string JobGroup { get; set; }

        public bool? IsNonConcurrent { get; set; }

        public bool? RequestsRecovery { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, EntryId };
        }
    }
}
