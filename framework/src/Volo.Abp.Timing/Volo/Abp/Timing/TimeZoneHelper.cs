using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneConverter;

namespace Volo.Abp.Timing;

public static class TimeZoneHelper
{
    public static List<NameValue> GetTimezones(List<NameValue> timezones)
    {
        return timezones
            .OrderBy(x => x.Name)
            .Select(x => new NameValue( $"{x.Name} ({GetTimezoneOffset(TZConvert.GetTimeZoneInfo(x.Name))})", x.Name))
            .ToList();
    }

    public static string GetTimezoneOffset(TimeZoneInfo timeZoneInfo)
    {
        if (timeZoneInfo.BaseUtcOffset < TimeSpan.Zero)
        {
            return "-" + timeZoneInfo.BaseUtcOffset.ToString(@"hh\:mm");
        }

        return "+" + timeZoneInfo.BaseUtcOffset.ToString(@"hh\:mm");
    }
}
