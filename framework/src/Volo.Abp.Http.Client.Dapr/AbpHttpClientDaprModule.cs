using Volo.Abp.Dapr;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.Dapr;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpDaprModule)
    )]
public class AbpHttpClientDaprModule : AbpModule
{
}