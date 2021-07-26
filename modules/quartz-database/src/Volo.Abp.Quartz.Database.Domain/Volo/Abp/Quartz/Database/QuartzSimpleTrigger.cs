using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_SIMPLE_TRIGGERS
    /// </summary>
    public class QuartzSimpleTrigger : Entity
    {
        public string SchedulerName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public long RepeatCount { get; set; }

        public long RepeatInterval { get; set; }

        public long TimesTriggered { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, TriggerName, TriggerGroup };
        }
    }
}
