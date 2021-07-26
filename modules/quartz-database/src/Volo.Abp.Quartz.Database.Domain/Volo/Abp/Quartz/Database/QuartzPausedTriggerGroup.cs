using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_PAUSED_TRIGGER_GRPS
    /// </summary>
    public class QuartzPausedTriggerGroup : Entity
    {
        public string SchedulerName { get; set; }

        public string TriggerGroup { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, TriggerGroup };
        }
    }
}
