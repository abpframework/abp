using Volo.Abp.Dapr;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpDaprModule)
)]
public class AbpAspNetCoreMvcDaprModule : AbpModule
{

}
