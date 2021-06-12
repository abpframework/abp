using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.Scriban
{
    [DependsOn(
        typeof(AbpTextTemplatingTestModule)
    )]
    public class ScribanTextTemplatingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ScribanTextTemplatingTestModule>("Volo.Abp.TextTemplating.Scriban");
            });
        }
    }
}
