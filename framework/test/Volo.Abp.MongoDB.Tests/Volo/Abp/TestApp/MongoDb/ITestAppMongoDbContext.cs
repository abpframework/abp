using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;

namespace Volo.Abp.TestApp.MongoDB;

[ConnectionStringName("TestApp")]
public interface ITestAppMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<Person> People { get; }

    IMongoCollection<City> Cities { get; }

    IMongoCollection<Product> Products { get; }

    IMongoCollection<AppEntityWithNavigations> AppEntityWithNavigations { get; }
}
