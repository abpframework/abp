using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(BookManagementDomainSharedModule)
        )]
    public class BookManagementDomainModule : AbpModule
    {

    }
}
