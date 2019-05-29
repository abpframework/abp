using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(DddApplicationModule)
        )]
    public class DocsApplicationContractsModule : AbpModule
    {
        
    }
}
