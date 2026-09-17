using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Mvc;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpMultiTenancyAbstractionsModule)
)]
public class AbpAspNetCoreMvcContractsModule : AbpModule
{

}
