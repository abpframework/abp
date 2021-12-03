using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization;

[DependsOn(
    typeof(AbpMultiTenancyModule)
)]
public class AbpAuthorizationAbstractionsModule : AbpModule
{

}
