using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_CRON_TRIGGERS
    /// </summary>
    public class QuartzCronTrigger : Entity
    {
        public string SchedulerName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public string CronExpression { get; set; }

        public string TimeZoneId { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, TriggerName, TriggerGroup };
        }
    }
}
