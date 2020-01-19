using System;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Acme.BookStore
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class BookStoreMongoDBModule: AbpModule
    {
    }
}
