using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Server;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class ClientProxyExceptionEventHandler : ILocalEventHandler<ClientProxyExceptionEventData>, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public ClientProxyExceptionEventHandler(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual async Task HandleEventAsync(ClientProxyExceptionEventData eventData)
    {
        if (eventData.StatusCode == 401)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreComponentsWebOptions>>();
                if (!options.Value.IsBlazorWebApp)
                {
                    var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
                    var accessTokenProvider = scope.ServiceProvider.GetRequiredService<IAccessTokenProvider>();
                    var result = await accessTokenProvider.RequestAccessToken();
                    if (result.Status != AccessTokenResultStatus.Success)
                    {
                        navigationManager.NavigateToLogout("authentication/logout");
                        return;
                    }

                    result.TryGetToken(out var token);
                    if (token != null && DateTimeOffset.Now >= token.Expires.AddMinutes(-5))
                    {
                        navigationManager.NavigateToLogout("authentication/logout");
                    }
                }
                else
                {
                    var jsRuntime = scope.ServiceProvider.GetRequiredService<IJSRuntime>();
                    await jsRuntime.InvokeVoidAsync("eval", "setTimeout(function(){location.assign('/')}, 2000)");
                }
            }
        }
    }
}
