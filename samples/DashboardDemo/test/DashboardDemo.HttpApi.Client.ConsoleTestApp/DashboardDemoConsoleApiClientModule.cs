using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace DashboardDemo.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(DashboardDemoHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class DashboardDemoConsoleApiClientModule : AbpModule
    {
        
    }
}
