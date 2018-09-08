using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(typeof(DocsDomainSharedModule))]
    public class DocsApplicationContractsModule : AbpModule
    {
        
    }
}
