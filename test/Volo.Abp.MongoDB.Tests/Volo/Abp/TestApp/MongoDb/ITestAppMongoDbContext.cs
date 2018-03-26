using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TestApp.MongoDb
{
    [ConnectionStringName("TestApp")]
    public interface ITestAppMongoDbContext : IAbpMongoDbContext
    {

    }
}