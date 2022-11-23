using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using Volo.Abp.AspNetCore.Components.Progression;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public class AbpMauiBlazorClientHttpMessageHandler : DelegatingHandler, ITransientDependency
{
    private readonly IUiPageProgressService _uiPageProgressService;
    
    private const string SelectedLanguageName = "Abp.SelectedLanguage";

    public AbpMauiBlazorClientHttpMessageHandler(IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor)
    {
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

            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            await _uiPageProgressService.Go(-1);
        }
    }

    private Task SetLanguageAsync(HttpRequestMessage request)
    {
        var selectedLanguage = Preferences.Get(SelectedLanguageName, string.Empty);

        if (!selectedLanguage.IsNullOrWhiteSpace())
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(selectedLanguage));
        }
        
        return Task.CompletedTask;
    }
}