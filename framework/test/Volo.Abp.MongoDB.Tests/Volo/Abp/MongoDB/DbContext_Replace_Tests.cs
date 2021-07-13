using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.MongoDB.TestApp.FourthContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.MongoDB;
using Xunit;

namespace Volo.Abp.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class DbContext_Replace_Tests : MongoDbTestBase
    {
        private readonly AbpMongoDbContextOptions _options;

        public DbContext_Replace_Tests()
        {
            _options = GetRequiredService<IOptions<AbpMongoDbContextOptions>>().Value;
        }

        [Fact]
        public void Should_Replace_DbContext()
        {
            _options.GetReplacedTypeOrSelf(typeof(IThirdDbContext)).ShouldBe(typeof(TestAppMongoDbContext));
            _options.GetReplacedTypeOrSelf(typeof(IFourthDbContext)).ShouldBe(typeof(TestAppMongoDbContext));

            (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppMongoDbContext).ShouldBeTrue();
            (ServiceProvider.GetRequiredService<IFourthDbContext>() is TestAppMongoDbContext).ShouldBeTrue();
        }
    }
}
