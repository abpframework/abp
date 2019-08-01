using Acme.BookStore.MongoDB;
using Volo.Abp.Modularity;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreMongoDbTestModule)
        )]
    public class BookStoreDomainTestModule : AbpModule
    {

    }
}