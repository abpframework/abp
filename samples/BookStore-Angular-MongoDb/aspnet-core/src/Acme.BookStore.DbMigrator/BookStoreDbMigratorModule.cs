using Acme.BookStore.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Acme.BookStore.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BookStoreMongoDbModule),
        typeof(BookStoreApplicationContractsModule)
        )]
    public class BookStoreDbMigratorModule : AbpModule
    {
        
    }
}
