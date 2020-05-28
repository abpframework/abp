using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Database
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(DatabaseEntityFrameworkCoreTestModule)
        )]
    public class DatabaseDomainTestModule : AbpModule
    {
        
    }
}
