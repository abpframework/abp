using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(BookManagementApplicationModule),
        typeof(BookManagementDomainTestModule)
        )]
    public class BookManagementApplicationTestModule : AbpModule
    {

    }
}
