using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule))]
    public class DocsHttpApiModule : AbpModule
    {
        
    }
}
