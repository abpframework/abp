using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(MyProjectNameHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class MyProjectNameConsoleApiClientModule : AbpModule
    {
        
    }
}
