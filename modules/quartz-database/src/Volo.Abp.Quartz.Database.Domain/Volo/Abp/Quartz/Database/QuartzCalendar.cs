using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Quartz.Database
{
    /// <summary>
    /// QRTZ_CALENDARS
    /// </summary>
    public class QuartzCalendar : Entity
    {
        public string SchedulerName { get; set; }

        public string CalendarName { get; set; }

        public byte[] Calendar { get; set; }

        public override object[] GetKeys()
        {
            return new[] { SchedulerName, CalendarName };
        }
    }
}
