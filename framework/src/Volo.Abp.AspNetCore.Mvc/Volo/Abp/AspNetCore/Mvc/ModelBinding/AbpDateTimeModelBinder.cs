using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding;

public class AbpDateTimeModelBinder : IModelBinder
{
    private readonly ILogger<AbpDateTimeModelBinder> _logger;
    private readonly DateTimeModelBinder _dateTimeModelBinder;
    private readonly IClock _clock;
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;
    private readonly ITimezoneProvider _timezoneProvider;

    public AbpDateTimeModelBinder(
        ILogger<AbpDateTimeModelBinder> logger,
        DateTimeModelBinder dateTimeModelBinder,
        IClock clock,
        ICurrentTimezoneProvider currentTimezoneProvider,
        ITimezoneProvider timezoneProvider)
    {
        _logger = logger;
        _dateTimeModelBinder = dateTimeModelBinder;
        _clock = clock;
        _currentTimezoneProvider = currentTimezoneProvider;
        _timezoneProvider = timezoneProvider;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        await _dateTimeModelBinder.BindModelAsync(bindingContext);

        if (!bindingContext.Result.IsModelSet || bindingContext.Result.Model is not DateTime dateTime)
        {
            return;
        }

        if (dateTime.Kind == DateTimeKind.Unspecified &&
            _clock.SupportsMultipleTimezone &&
            !_currentTimezoneProvider.TimeZone.IsNullOrWhiteSpace())
        {
            try
            {
                var timezoneInfo = _timezoneProvider.GetTimeZoneInfo(_currentTimezoneProvider.TimeZone);
                dateTime = new DateTimeOffset(dateTime, timezoneInfo.GetUtcOffset(dateTime)).UtcDateTime;
            }
            catch
            {
                _logger.LogWarning("Could not convert DateTime with unspecified Kind using timezone '{TimeZone}'.", _currentTimezoneProvider.TimeZone);
            }
        }

        bindingContext.Result = ModelBindingResult.Success(_clock.Normalize(dateTime));
    }
}
