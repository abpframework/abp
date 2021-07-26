using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_TRIGGERS
    /// </summary>
    public class QuartzTrigger : Entity
    {
        public string SchedulerName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public string JobName { get; set; }

        public string JobGroup { get; set; }

        public string Description { get; set; }

        public long? NextFireTime { get; set; }

        public long? PreviousFireTime { get; set; }

        public int? Priority { get; set; }

        public string TriggerState { get; set; }

        public string TriggerType { get; set; }

        public long StartTime { get; set; }

        public long? EndTime { get; set; }

        public string CalendarName { get; set; }

        public short? MisfireInstruction { get; set; }

        public byte[] JobData { get; set; }

        public ICollection<QuartzSimpleTrigger> SimpleTriggers { get; set; }

        public ICollection<QuartzSimplePropertyTrigger> SimplePropertyTriggers { get; set; }

        public ICollection<QuartzCronTrigger> CronTriggers { get; set; }

        public ICollection<QuartzBlobTrigger> BlobTriggers { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, TriggerName, TriggerGroup };
        }
    }
}
