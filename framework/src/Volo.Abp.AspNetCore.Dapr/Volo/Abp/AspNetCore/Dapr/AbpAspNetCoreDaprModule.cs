using Volo.Abp.Dapr;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Dapr;

[DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(AbpDaprModule)
    )]
public class AbpAspNetCoreDaprModule : AbpModule
{
    
}