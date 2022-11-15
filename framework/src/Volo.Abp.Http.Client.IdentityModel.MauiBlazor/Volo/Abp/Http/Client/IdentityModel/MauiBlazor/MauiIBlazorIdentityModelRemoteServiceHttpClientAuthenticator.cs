using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;

namespace Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

[Dependency(ReplaceServices = true)]
public class MauiBlazorIdentityModelRemoteServiceHttpClientAuthenticator : IdentityModelRemoteServiceHttpClientAuthenticator
{
    [CanBeNull]
    protected IAbpMauiAccessTokenProvider AbpMauiAccessTokenProvider { get; }

    public MauiBlazorIdentityModelRemoteServiceHttpClientAuthenticator(
        IIdentityModelAuthenticationService identityModelAuthenticationService,
        IServiceProvider serviceProvider)
        : base(identityModelAuthenticationService)
    {
        AbpMauiAccessTokenProvider = serviceProvider.GetService<IAbpMauiAccessTokenProvider>();
    }

    public async override Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
    {
        if (context.RemoteService.GetUseCurrentAccessToken() != false)
        {
            var accessToken = await GetAccessTokenFromAccessTokenProviderOrNullAsync();
            if (accessToken != null)
            {
                context.Request.SetBearerToken(accessToken);
                return;
            }
        }

        await base.Authenticate(context);
    }

    protected virtual async Task<string> GetAccessTokenFromAccessTokenProviderOrNullAsync()
    {
        if (AbpMauiAccessTokenProvider == null)
        {
            return null;
        }

        return await AbpMauiAccessTokenProvider.GetAccessTokenAsync();
    }
}
