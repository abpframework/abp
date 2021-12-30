using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MongoDB.TestApp.FourthContext;
using Volo.Abp.MongoDB.TestApp.SecondContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.MongoDB;
using Xunit;

namespace Volo.Abp.MongoDB;

[Collection(MongoTestCollection.Name)]
public class AbpMongoDbConventionalRegistrar_Tests : MongoDbTestBase
{
    [Fact]
    public void All_AbpMongoDbContext_Should_Exposed_IAbpMongoDbContext_Service()
    {
        var abpMongoDbContext = ServiceProvider.GetServices<IAbpMongoDbContext>();
        abpMongoDbContext.ShouldContain(x => x is TestAppMongoDbContext);
        abpMongoDbContext.ShouldContain(x => x is SecondDbContext);
        abpMongoDbContext.ShouldContain(x => x is ThirdDbContext);
        abpMongoDbContext.ShouldContain(x => x is FourthDbContext);
    }
}
