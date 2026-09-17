using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Memory;

[DependsOn(
    typeof(AbpBlobStoringMemoryModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringMemoryTestModule : AbpModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseMemory();
            });
        });
    }
}
