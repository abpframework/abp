using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization;

[DependsOn(
    typeof(AbpMultiTenancyAbstractionsModule)
)]
public class AbpAuthorizationAbstractionsModule : AbpModule
{

}
