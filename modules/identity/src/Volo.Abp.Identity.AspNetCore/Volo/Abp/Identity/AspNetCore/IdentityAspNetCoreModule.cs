using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.AspNetCore
{
    [DependsOn(
        typeof(IdentityDomainModule)
        )]
    public class IdentityAspNetCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services
                .GetObject<IdentityBuilder>()
                .AddDefaultTokenProviders()
                .AddSignInManager();

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
    }
}
