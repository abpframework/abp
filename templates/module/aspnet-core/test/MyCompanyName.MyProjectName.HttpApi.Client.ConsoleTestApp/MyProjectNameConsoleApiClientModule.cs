using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class MyProjectNameConsoleApiClientModule : AbpModule
    {
        
    }
}
