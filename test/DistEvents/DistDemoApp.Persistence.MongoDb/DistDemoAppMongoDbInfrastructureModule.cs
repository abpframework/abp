using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace DistDemoApp;

[DependsOn(
    typeof(AbpMongoDbModule),
    typeof(DistDemoAppSharedModule)
)]
public class DistDemoAppMongoDbInfrastructureModule : AbpModule
{
}
