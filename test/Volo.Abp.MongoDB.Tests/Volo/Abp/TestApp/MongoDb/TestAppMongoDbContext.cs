using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDb
{
    [ConnectionStringName("TestApp")]
    public class TestAppMongoDbContext : AbpMongoDbContext, ITestAppMongoDbContext
    {
        public override IReadOnlyList<MongoEntityMapping> GetMappings()
        {
            return new[]
            {
                new MongoEntityMapping(typeof(Person), "People") 
            };
        }
    }
}
