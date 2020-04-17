using Volo.Abp.Modularity;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsAdminApplicationContractsModule))]
    public class DocsAdminHttpApiClientModule : AbpModule
    {
        //TODO: Create client proxies!
    }
}
