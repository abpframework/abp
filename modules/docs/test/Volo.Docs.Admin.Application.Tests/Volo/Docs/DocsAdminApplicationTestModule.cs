using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Memory;
using Volo.Abp.Modularity;
using Volo.Docs.Admin;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsAdminApplicationModule),
        typeof(DocsDomainTestModule),
        typeof(AbpBlobStoringMemoryModule)
    )]
    public class DocsAdminApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseMemory();
                });
            });
        }
    }
}
