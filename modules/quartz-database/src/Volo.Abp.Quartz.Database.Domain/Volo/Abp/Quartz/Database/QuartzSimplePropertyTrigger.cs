using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_SIMPROP_TRIGGERS
    /// </summary>
    public class QuartzSimplePropertyTrigger : Entity
    {
        public string SchedulerName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public string StringProperty1 { get; set; }

        public string StringProperty2 { get; set; }

        public string StringProperty3 { get; set; }

        public int? IntegerProperty1 { get; set; }

        public int? IntegerProperty2 { get; set; }

        public long? LongProperty1 { get; set; }

        public long? LongProperty2 { get; set; }

        public decimal? DecimalProperty1 { get; set; }

        public decimal? DecimalProperty2 { get; set; }

        public bool? BooleanProperty1 { get; set; }

        public bool? BooleanProperty2 { get; set; }

        public string TimeZoneId { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, TriggerName, TriggerGroup };
        }
    }
}
