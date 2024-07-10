using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer;

[DependsOn(typeof(AbpSecurityModule), typeof(AbpCachingModule), typeof(AbpAspNetCoreAbstractionsModule))]
public class AbpAspNetCoreAuthenticationJwtBearerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
        context.Services.AddHttpContextAccessor();

        if (context.Services.ExecutePreConfiguredActions<WebRemoteDynamicClaimsPrincipalContributorOptions>().IsEnabled &&
            context.Services.ExecutePreConfiguredActions<AbpClaimsPrincipalFactoryOptions>().IsRemoteRefreshEnabled)
        {
            context.Services.AddTransient<WebRemoteDynamicClaimsPrincipalContributor>();
            context.Services.AddTransient<WebRemoteDynamicClaimsPrincipalContributorCache>();
        }

    }
}
