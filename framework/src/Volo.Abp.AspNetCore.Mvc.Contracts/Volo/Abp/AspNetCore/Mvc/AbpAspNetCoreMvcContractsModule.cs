using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc;

[DependsOn(
    typeof(AbpDddApplicationContractsModule)
    )]
public class AbpAspNetCoreMvcContractsModule : AbpModule
{

}
