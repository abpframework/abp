using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Ldap.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Ldap
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpLocalizationModule))]
    public class AbpLdapModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDynamicOptions<AbpLdapOptions, AbpAbpLdapOptionsManager>();

            var configuration = context.Services.GetConfiguration();
            var ldapConfiguration = configuration["Ldap"];
            if (!ldapConfiguration.IsNullOrEmpty())
            {
                Configure<AbpLdapOptions>(configuration.GetSection("Ldap"));
            }

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpLdapModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<LdapResource>("en")
                    .AddVirtualJson("/Volo/Abp/Ldap/Localization");
            });
        }
    }
}
