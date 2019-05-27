using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule),
        typeof(AspNetCoreMvcModule)
        )]
    public class DocsHttpApiModule : AbpModule
    {
        
    }
}
