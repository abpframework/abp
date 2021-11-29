using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class CmsKitConsoleApiClientModule : AbpModule
    {
        
    }
}
