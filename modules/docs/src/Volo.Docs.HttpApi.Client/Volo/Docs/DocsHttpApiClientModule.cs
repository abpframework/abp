using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule))]
    public class DocsHttpApiClientModule : AbpModule
    {
        //TODO: Create client proxies
    }
}
