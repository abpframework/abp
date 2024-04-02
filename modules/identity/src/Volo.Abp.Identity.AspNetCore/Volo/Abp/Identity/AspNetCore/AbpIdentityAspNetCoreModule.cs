using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using static Volo.Abp.Identity.AspNetCore.AbpSecurityStampValidatorCallback;

namespace Volo.Abp.Identity.AspNetCore;

[DependsOn(
    typeof(AbpIdentityDomainModule)
    )]
public class AbpIdentityAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder
                .AddDefaultTokenProviders()
                .AddTokenProvider<LinkUserTokenProvider>(LinkUserTokenProviderConsts.LinkUserTokenProviderName)
                .AddSignInManager<AbpSignInManager>()
                .AddUserValidator<AbpIdentityUserValidator>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //(TODO: Extract an extension method like IdentityBuilder.AddAbpSecurityStampValidator())
        context.Services.AddScoped<AbpSecurityStampValidator>();
        context.Services.AddScoped(typeof(SecurityStampValidator<IdentityUser>), provider => provider.GetService(typeof(AbpSecurityStampValidator)));
        context.Services.AddScoped(typeof(ISecurityStampValidator), provider => provider.GetService(typeof(AbpSecurityStampValidator)));

        var options = context.Services.ExecutePreConfiguredActions(new AbpIdentityAspNetCoreOptions());

        if (options.ConfigureAuthentication)
        {
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<SecurityStampValidatorOptions>()
            .Configure<IServiceProvider>((securityStampValidatorOptions, serviceProvider) =>
            {
                var abpRefreshingPrincipalOptions = serviceProvider.GetRequiredService<IOptions<AbpRefreshingPrincipalOptions>>().Value;
                securityStampValidatorOptions.UpdatePrincipal(abpRefreshingPrincipalOptions);
            });
    }
}
