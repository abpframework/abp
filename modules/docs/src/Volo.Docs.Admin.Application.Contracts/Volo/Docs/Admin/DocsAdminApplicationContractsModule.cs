using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Localization;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class DocsAdminApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsAdminApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<DocsResource>()
                    .AddVirtualJson("Volo/Docs/Admin/Localization/Resources/Docs/ApplicationContracts");
            });
        }
    }
}
