using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer;

[DependsOn(typeof(AbpSecurityModule), typeof(AbpCachingModule))]
public class AbpAspNetCoreAuthenticationJwtBearerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
        context.Services.AddHttpContextAccessor();
        var abpClaimsPrincipalFactoryOptions = context.Services.ExecutePreConfiguredActions<AbpClaimsPrincipalFactoryOptions>();
        if (abpClaimsPrincipalFactoryOptions.IsRemoteRefreshEnabled)
        {
            context.Services.AddTransient<WebRemoteDynamicClaimsPrincipalContributor>();
            context.Services.AddTransient<WebRemoteDynamicClaimsPrincipalContributorCache>();
        }
    }
}
