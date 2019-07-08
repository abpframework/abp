using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpAccountHttpApiModule : AbpModule
    {

    }
}