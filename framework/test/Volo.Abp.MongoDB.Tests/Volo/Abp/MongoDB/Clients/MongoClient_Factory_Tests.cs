using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace Volo.Abp.MongoDB.Clients;

[Collection(MongoTestCollection.Name)]
public class MongoClient_Factory_Tests : MongoDbTestBase
{
    private readonly IAbpMongoClientFactory _factory;

    public MongoClient_Factory_Tests()
    {
        _factory = GetRequiredService<IAbpMongoClientFactory>();
    }

    [Fact]
    public async Task Should_Return_Same_Instance_For_Same_ConnectionString()
    {
        // Arrange
        var mongoUrl = new MongoUrl("mongodb://localhost:27017/my-db");

        // Act
        var client1 = await _factory.GetAsync(mongoUrl);
        var client2 = await _factory.GetAsync(mongoUrl);

        // Assert
        Assert.Same(client1, client2);
    }

    [Fact]
    public async Task Should_Return_Different_Instances_For_Different_ConnectionStrings()
    {
        // Arrange
        var mongoUrl1 = new MongoUrl("mongodb://localhost:27017/db1");
        var mongoUrl2 = new MongoUrl("mongodb://localhost:27017/db2");

        // Act
        var client1 = await _factory.GetAsync(mongoUrl1);
        var client2 = await _factory.GetAsync(mongoUrl2);

        // Assert
        Assert.NotSame(client1, client2);
    }

    [Fact]
    public async Task Should_Not_Throw_For_Valid_But_Unreachable_Connection()
    {
        // Arrange
        var mongoUrl = new MongoUrl("mongodb://unreachablehost:12345/any");

        // Act
        var client = await _factory.GetAsync(mongoUrl);

        // Assert
        Assert.NotNull(client); // Even though it's not connectable now, the instance can be created
    }

    [Fact]
    public async Task Should_Be_ThreadSafe_When_Accessed_Concurrently()
    {
        var mongoUrl = new MongoUrl("mongodb://localhost:27017/threadsafe");
        var results = new MongoClient[100];

        await Parallel.ForAsync(0, 100, async (i, _) =>
        {
            results[i] = await _factory.GetAsync(mongoUrl);
        });

        Assert.All(results, client => Assert.Same(results[0], client));
    }

    [Fact]
    public async Task Should_Throw_If_ConnectionString_Is_Null_Or_Empty()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _factory.GetAsync(null!));
    }
}
