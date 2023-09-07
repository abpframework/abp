using Volo.BookStore.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.BookStore.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BookStoreMongoDbModule),
    typeof(BookStoreApplicationContractsModule)
    )]
public class BookStoreDbMigratorModule : AbpModule
{
}
