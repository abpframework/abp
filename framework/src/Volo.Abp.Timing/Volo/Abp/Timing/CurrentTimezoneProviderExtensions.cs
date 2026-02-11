using System;

namespace Volo.Abp.Timing;

public static class CurrentTimezoneProviderExtensions
{
    public static IDisposable Change(this ICurrentTimezoneProvider currentTimezoneProvider, string? timeZone)
    {
        var parentScope = currentTimezoneProvider.TimeZone;
        currentTimezoneProvider.TimeZone = timeZone;

        return new DisposeAction<ValueTuple<ICurrentTimezoneProvider, string?>>(static (state) =>
        {
            var (currentTimezoneProvider, parentScope) = state;
            currentTimezoneProvider.TimeZone = parentScope;
        }, (currentTimezoneProvider, parentScope));
    }
}
