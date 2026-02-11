using Volo.Abp.Ldap.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Ldap;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpLocalizationModule))]
public class AbpLdapAbstractionsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpLdapAbstractionsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<LdapResource>("en")
                .AddVirtualJson("/Volo/Abp/Ldap/Localization");
        });
    }
}
