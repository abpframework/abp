using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_JOB_DETAILS
    /// </summary>
    public class QuartzJobDetail : Entity
    {
        public string SchedulerName { get; set; }

        public string JobName { get; set; }

        public string JobGroup { get; set; }

        public string Description { get; set; }

        public string JobClassName { get; set; }

        public bool IsDurable { get; set; }

        public bool IsNonConcurrent { get; set; }

        public bool IsUpdateData { get; set; }

        public bool RequestsRecovery { get; set; }

        public byte[] JobData { get; set; }

        public ICollection<QuartzTrigger> Triggers { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { SchedulerName, JobName, JobGroup };
        }
    }
}
