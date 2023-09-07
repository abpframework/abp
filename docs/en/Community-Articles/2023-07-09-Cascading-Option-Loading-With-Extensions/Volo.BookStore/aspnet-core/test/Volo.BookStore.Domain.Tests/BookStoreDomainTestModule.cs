using Volo.BookStore.MongoDB;
using Volo.Abp.Modularity;

namespace Volo.BookStore;

[DependsOn(
    typeof(BookStoreMongoDbTestModule)
    )]
public class BookStoreDomainTestModule : AbpModule
{

}
