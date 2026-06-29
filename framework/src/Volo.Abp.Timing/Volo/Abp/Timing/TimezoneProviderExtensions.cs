using System;

namespace Volo.Abp.Timing;

public static class TimezoneProviderExtensions
{
    /// <summary>
    /// Interprets <paramref name="dateTime"/> as local time in <paramref name="windowsOrIanaTimeZoneId"/>
    /// and converts it to its UTC equivalent. The caller is expected to pass a
    /// <see cref="DateTimeKind.Unspecified"/> value; the kind is not inspected, so a value is always
    /// treated as wall-clock time in the given timezone regardless of its kind.
    /// </summary>
    /// <remarks>
    /// Returns <paramref name="dateTime"/> unchanged when applying the timezone offset would move it
    /// outside the supported <see cref="DateTime"/> range. This happens for values within the offset
    /// distance of <see cref="DateTime.MinValue"/>/<see cref="DateTime.MaxValue"/>, typically the
    /// <see cref="DateTime.MinValue"/> placeholder that does not represent a real instant. Computing the
    /// UTC ticks directly avoids the <see cref="ArgumentOutOfRangeException"/> that
    /// <c>new DateTimeOffset(dateTime, offset)</c> would throw for such values.
    /// </remarks>
    public static DateTime ConvertUnspecifiedToUtc(
        this ITimezoneProvider timezoneProvider,
        DateTime dateTime,
        string windowsOrIanaTimeZoneId)
    {
        Check.NotNull(timezoneProvider, nameof(timezoneProvider));

        var timezoneInfo = timezoneProvider.GetTimeZoneInfo(windowsOrIanaTimeZoneId);
        var utcTicks = dateTime.Ticks - timezoneInfo.GetUtcOffset(dateTime).Ticks;
        if (utcTicks < DateTime.MinValue.Ticks || utcTicks > DateTime.MaxValue.Ticks)
        {
            return dateTime;
        }

        return new DateTime(utcTicks, DateTimeKind.Utc);
    }
}
