using Volo.Abp.Application.Localization.Resources.AbpDdd;
using Volo.Abp.Auditing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Application
{
    [DependsOn(
        typeof(AbpAuditingModule),
        typeof(AbpLocalizationModule)
        )]
    public class AbpDddApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpDddApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpDddApplicationContractsResource>("en")
                    .AddVirtualJson("/Volo/Abp/Application/Localization/Resources/AbpDdd");
            });
        }
    }
}
