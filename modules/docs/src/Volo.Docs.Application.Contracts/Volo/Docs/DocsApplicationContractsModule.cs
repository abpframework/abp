using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Docs.Common;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(DocsCommonApplicationContractsModule),
        typeof(AbpDddApplicationContractsModule)
        )]
    public class DocsApplicationContractsModule : AbpModule
    {
        
    }
}
