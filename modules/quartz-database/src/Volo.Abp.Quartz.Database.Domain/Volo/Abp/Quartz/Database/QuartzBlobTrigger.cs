using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_BLOB_TRIGGERS
    /// </summary>
    public class QuartzBlobTrigger : Entity
    {
        public string SchedulerName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public byte[] BlobData { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, TriggerName, TriggerGroup };
        }
    }
}