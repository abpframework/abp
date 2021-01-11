using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(CmsKitCommonApplicationContractsModule)
        )]
    public class CmsKitCommonHttpApiClientModule : AbpModule
    {
    }
}
