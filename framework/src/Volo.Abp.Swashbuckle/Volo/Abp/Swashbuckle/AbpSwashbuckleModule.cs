using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Swashbuckle
{
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class AbpSwashbuckleModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSwashbuckleModule>();
            });
        }
    }
}
