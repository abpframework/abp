using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class AbpOpenIddictAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableTokenEndpointPassthrough()
                .EnableUserinfoEndpointPassthrough()
                .EnableLogoutEndpointPassthrough()
                .EnableVerificationEndpointPassthrough();
        });

        PreConfigure<OpenIddictValidationBuilder>(builder =>
        {
            builder.UseAspNetCore();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Insert(0, new AbpOpenIddictTenantResolveContributor());
        });

        Configure<AbpOpenIddictClaimDestinationsOptions>(options =>
        {
            options.ClaimDestinationsProvider.Add<AbpDefaultOpenIddictClaimDestinationsProvider>();
        });

        Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Volo/Abp/OpenIddict/Views/{1}/{0}.cshtml");
        });
    }
}
