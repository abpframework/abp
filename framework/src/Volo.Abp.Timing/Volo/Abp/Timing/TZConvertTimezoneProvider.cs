using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneConverter;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Timing
{
    public class TZConvertTimezoneProvider : ITimezoneProvider, ITransientDependency
    {
        public virtual List<NameValue> GetWindowsTimezones()
        {
            return TZConvert.KnownWindowsTimeZoneIds.OrderBy(x => x).Select(x => new NameValue(x, x)).ToList();
        }

        public virtual List<NameValue> GetIanaTimezones()
        {
            return TZConvert.KnownIanaTimeZoneNames.OrderBy(x => x).Select(x => new NameValue(x, x)).ToList();
        }

        public virtual string WindowsToIana(string windowsTimeZoneId)
        {
            return TZConvert.WindowsToIana(windowsTimeZoneId);
        }

        public virtual string IanaToWindows(string ianaTimeZoneName)
        {
            return TZConvert.IanaToWindows(ianaTimeZoneName);
        }

        public virtual TimeZoneInfo GetTimeZoneInfo(string windowsOrIanaTimeZoneId)
        {
            return TZConvert.GetTimeZoneInfo(windowsOrIanaTimeZoneId);
        }
    }
}
