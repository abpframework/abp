using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Admin
{
    [DependsOn(typeof(BloggingDomainSharedModule))]
    public class BloggingAdminApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingAdminApplicationContractsModule>();
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BloggingResource>()
                    .AddVirtualJson("Volo/Blogging/Admin/Localization/Resources/Blogging/Admin/ApplicationContracts");
            });
        }
    }
}
