using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(IdentityApplicationContractsModule), typeof(AspNetCoreMvcModule))]
    public class IdentityHttpApiModule : AbpModule
    {
        
    }
}