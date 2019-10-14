using Acme.BookStore.BookManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(BookManagementEntityFrameworkCoreTestModule)
        )]
    public class BookManagementDomainTestModule : AbpModule
    {
        
    }
}
