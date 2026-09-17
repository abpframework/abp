using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public class MauiBlazorCurrentTimezoneService : ITransientDependency
{
    protected IClock Clock { get; }
    protected ICachedApplicationConfigurationClient ApplicationConfigurationClient { get; }
    protected IJSRuntime JsRuntime { get; }
    protected ICurrentTimezoneProvider CurrentTimezoneProvider { get; }

    public MauiBlazorCurrentTimezoneService(
        IClock clock,
        ICachedApplicationConfigurationClient applicationConfigurationClient,
        IJSRuntime jsRuntime,
        ICurrentTimezoneProvider currentTimezoneProvider)
    {
        Clock = clock;
        ApplicationConfigurationClient = applicationConfigurationClient;
        JsRuntime = jsRuntime;
        CurrentTimezoneProvider = currentTimezoneProvider;
    }

    public virtual async Task InitializeAsync()
    {
        if (Clock.SupportsMultipleTimezone)
        {
            var configurationDto = await ApplicationConfigurationClient.GetAsync();
            CurrentTimezoneProvider.TimeZone = !configurationDto.Timing.TimeZone.Iana.TimeZoneName.IsNullOrEmpty()
                ? configurationDto.Timing.TimeZone.Iana.TimeZoneName
                : await JsRuntime.InvokeAsync<string>("abp.clock.getBrowserTimeZone");

            await JsRuntime.InvokeAsync<string>("abp.clock.setBrowserTimeZoneToCookie");
        }
    }
}
