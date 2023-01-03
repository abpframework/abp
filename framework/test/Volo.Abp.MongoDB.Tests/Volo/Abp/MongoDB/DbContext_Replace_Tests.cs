using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MongoDB.TestApp.FourthContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.MongoDB;
using Xunit;

namespace Volo.Abp.MongoDB;

[Collection(MongoTestCollection.Name)]
public class DbContext_Replace_Tests : MongoDbTestBase
{
    private readonly IMongoDbContextTypeProvider _dbContextTypeProvider;

    public DbContext_Replace_Tests()
    {
        _dbContextTypeProvider = GetRequiredService<IMongoDbContextTypeProvider>();
    }

    [Fact]
    public async Task Should_Replace_DbContext()
    {
        (await _dbContextTypeProvider.GetDbContextTypeAsync(typeof(IThirdDbContext))).ShouldBe(typeof(TestAppMongoDbContext));
        (await _dbContextTypeProvider.GetDbContextTypeAsync(typeof(IFourthDbContext))).ShouldBe(typeof(TestAppMongoDbContext));

        (ServiceProvider.GetRequiredService<IThirdDbContext>() is TestAppMongoDbContext).ShouldBeTrue();
        (ServiceProvider.GetRequiredService<IFourthDbContext>() is TestAppMongoDbContext).ShouldBeTrue();
    }
}
