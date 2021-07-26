using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_LOCKS
    /// </summary>
    public class QuartzLock : Entity
    {
        public string SchedulerName { get; set; }

        public string LockName { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, LockName };
        }
    }
}
