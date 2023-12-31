using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.RemoteServices;

namespace Volo.Abp.Http.Client;

[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(AbpRemoteServicesModule)
)]
public class AbpRemoteServicesTestModule: AbpModule
{
}