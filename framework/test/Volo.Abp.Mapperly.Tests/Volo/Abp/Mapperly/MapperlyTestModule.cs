using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Mapperly;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpObjectExtendingTestModule)
)]
public class MapperlyTestModule : AbpModule
{

}
