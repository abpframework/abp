using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsAdminApplicationContractsModule),
        typeof(AspNetCoreMvcModule)
        )]
    public class DocsAdminHttpApiModule : AbpModule
    {
        
    }
}
