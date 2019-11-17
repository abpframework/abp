using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(BookManagementHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class BookManagementConsoleApiClientModule : AbpModule
    {
        
    }
}
