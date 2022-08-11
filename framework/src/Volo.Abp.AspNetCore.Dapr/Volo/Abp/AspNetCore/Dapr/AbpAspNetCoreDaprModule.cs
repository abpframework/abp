using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Dapr;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule)
)]
public class AbpAspNetCoreDaprModule : AbpModule
{

}
