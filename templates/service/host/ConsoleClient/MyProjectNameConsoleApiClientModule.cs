using MyCompanyName.MyProjectName;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace ConsoleClient
{
    [DependsOn(
        typeof(MyProjectNameHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class MyProjectNameConsoleApiClientModule : AbpModule
    {
        
    }
}
