using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web;
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
        using (var scope = ServiceProvider.CreateScope())
        {
            if (eventData.StatusCode == 401)
            {
                var options = scope.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreComponentsWebOptions>>();
                if (!options.Value.IsBlazorWebApp)
                {
                    var authenticationOptions = scope.ServiceProvider.GetRequiredService<IOptions<AbpAuthenticationOptions>>();
                    var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
                    navigationManager.NavigateToLogout(authenticationOptions.Value.LogoutUrl, "/");
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
