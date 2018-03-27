using System.Collections.Generic;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDb
{
    [ConnectionStringName("TestApp")]
    public class TestAppMongoDbContext : AbpMongoDbContext, ITestAppMongoDbContext
    {
        //TODO: We can set collections automatically, lik EF Core
        //TODO: We can get collection names conventionally, or by an attribute

        public IMongoCollection<Person> People => Collection<Person>();

        public IMongoCollection<City> Cities => Collection<City>();

        //TODO: Default implementation should read IMongoCollections from the context!
        //GetMappings should send a context and we add to it. Rename to ConfigureMappings.

        public override IReadOnlyList<MongoEntityMapping> GetMappings()
        {
            return new[]
            {
                new MongoEntityMapping(typeof(Person), "People"),
                new MongoEntityMapping(typeof(City), "Cities")
            };
        }
    }
}
