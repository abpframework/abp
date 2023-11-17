using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;

[DependsOn(
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpCachingModule)
)]
public class AbpAspNetCoreAuthenticationJwtBearerDynamicClaimsModule : AbpModule
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
