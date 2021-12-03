using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDB;

[ConnectionStringName("TestApp")]
public interface ITestAppMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<Person> People { get; }

    IMongoCollection<City> Cities { get; }
}
