using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Progression;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public class AbpMauiBlazorClientHttpMessageHandler : DelegatingHandler, ITransientDependency
{
    private readonly IUiPageProgressService _uiPageProgressService;
    private readonly IMauiBlazorSelectedLanguageProvider _mauiBlazorSelectedLanguageProvider;
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;

    public AbpMauiBlazorClientHttpMessageHandler(
        IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor,
        IMauiBlazorSelectedLanguageProvider mauiBlazorSelectedLanguageProvider,
        ICurrentTimezoneProvider currentTimezoneProvider)
    {
        _mauiBlazorSelectedLanguageProvider = mauiBlazorSelectedLanguageProvider;
        _currentTimezoneProvider = currentTimezoneProvider;
        _uiPageProgressService = clientScopeServiceProviderAccessor.ServiceProvider.GetRequiredService<IUiPageProgressService>();
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            await _uiPageProgressService.Go(null, options =>
            {
                options.Type = UiPageProgressType.Info;
            });

            await SetLanguageAsync(request);
            await SetTimeZoneAsync(request);

            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            await _uiPageProgressService.Go(-1);
        }
    }

    private async Task SetLanguageAsync(HttpRequestMessage request)
    {
        var selectedLanguage = await _mauiBlazorSelectedLanguageProvider.GetSelectedLanguageAsync();

        if (!selectedLanguage.IsNullOrWhiteSpace())
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(selectedLanguage!));
        }
    }

    private Task SetTimeZoneAsync(HttpRequestMessage request)
    {
        if (!_currentTimezoneProvider.TimeZone.IsNullOrWhiteSpace())
        {
            request.Headers.Remove(TimeZoneConsts.DefaultTimeZoneKey);
            request.Headers.Add(TimeZoneConsts.DefaultTimeZoneKey, _currentTimezoneProvider.TimeZone);
        }

        return Task.CompletedTask;
    }
}
