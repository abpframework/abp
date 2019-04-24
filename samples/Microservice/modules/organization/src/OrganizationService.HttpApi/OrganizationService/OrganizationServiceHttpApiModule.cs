using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace OrganizationService
{
    [DependsOn(
        typeof(OrganizationServiceApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class OrganizationServiceHttpApiModule : AbpModule
    {
        
    }
}
