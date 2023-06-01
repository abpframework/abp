using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class ClientProxyExceptionEventHandler : ILocalEventHandler<ClientProxyExceptionEventData>, ITransientDependency
{
    protected NavigationManager NavigationManager { get; }
    protected IAccessTokenProvider AccessTokenProvider { get; }

    public ClientProxyExceptionEventHandler(NavigationManager navigationManager, IAccessTokenProvider accessTokenProvider)
    {
        NavigationManager = navigationManager;
        AccessTokenProvider = accessTokenProvider;
    }

    public virtual async Task HandleEventAsync(ClientProxyExceptionEventData eventData)
    {
        if (eventData.StatusCode == 401)
        {
            var result = await AccessTokenProvider.RequestAccessToken();
            if (result.Status != AccessTokenResultStatus.Success)
            {
                NavigationManager.NavigateToLogout("authentication/logout");
                return;
            }

            result.TryGetToken(out var token);
            if (token != null && DateTimeOffset.Now >= token.Expires.AddMinutes(-5))
            {
                NavigationManager.NavigateToLogout("authentication/logout");
            }
        }
    }
}
