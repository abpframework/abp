using System;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OperationRateLimiting;

public class DefaultOperationRateLimitingFormatter
    : IOperationRateLimitingFormatter, ITransientDependency
{
    protected IStringLocalizer<AbpOperationRateLimitingResource> Localizer { get; }

    public DefaultOperationRateLimitingFormatter(
        IStringLocalizer<AbpOperationRateLimitingResource> localizer)
    {
        Localizer = localizer;
    }

    public virtual string Format(TimeSpan duration)
    {
        if (duration.TotalDays >= 365)
        {
            var years = (int)(duration.TotalDays / 365);
            var remainingDays = (int)(duration.TotalDays % 365);
            var months = remainingDays / 30;
            return months > 0
                ? Localizer["RetryAfter:YearsAndMonths", years, months]
                : Localizer["RetryAfter:Years", years];
        }

        if (duration.TotalDays >= 30)
        {
            var months = (int)(duration.TotalDays / 30);
            var remainingDays = (int)(duration.TotalDays % 30);
            return remainingDays > 0
                ? Localizer["RetryAfter:MonthsAndDays", months, remainingDays]
                : Localizer["RetryAfter:Months", months];
        }

        if (duration.TotalDays >= 1)
        {
            var days = (int)duration.TotalDays;
            var hours = duration.Hours;
            return hours > 0
                ? Localizer["RetryAfter:DaysAndHours", days, hours]
                : Localizer["RetryAfter:Days", days];
        }

        if (duration.TotalHours >= 1)
        {
            var hours = (int)duration.TotalHours;
            var minutes = duration.Minutes;
            return minutes > 0
                ? Localizer["RetryAfter:HoursAndMinutes", hours, minutes]
                : Localizer["RetryAfter:Hours", hours];
        }

        if (duration.TotalMinutes >= 1)
        {
            var minutes = (int)duration.TotalMinutes;
            var seconds = duration.Seconds;
            return seconds > 0
                ? Localizer["RetryAfter:MinutesAndSeconds", minutes, seconds]
                : Localizer["RetryAfter:Minutes", minutes];
        }

        return Localizer["RetryAfter:Seconds", (int)duration.TotalSeconds];
    }
}
