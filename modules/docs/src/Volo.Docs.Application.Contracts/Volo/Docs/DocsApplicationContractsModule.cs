using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class DocsApplicationContractsModule : AbpModule
    {
        
    }
}
