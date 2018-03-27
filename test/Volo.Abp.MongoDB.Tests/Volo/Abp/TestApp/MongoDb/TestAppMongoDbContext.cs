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
        public IMongoCollection<Person> People { get; set; }

        public IMongoCollection<City> Cities { get; set; }

        //TODO: Default implementation should read mogo collections from the context!
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
