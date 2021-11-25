using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Json;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpJsonModule),
    typeof(AbpTestBaseModule)
)]
public class AbpJsonTestModule : AbpModule
{

}
