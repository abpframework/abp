using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Docs.Localization;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddDomainModule)
        )]
    public class DocsDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DocsDomainModule>();
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<DocsResource>().AddVirtualJson("/Volo/Docs/Localization/Domain");
            });
        }
    }
}
