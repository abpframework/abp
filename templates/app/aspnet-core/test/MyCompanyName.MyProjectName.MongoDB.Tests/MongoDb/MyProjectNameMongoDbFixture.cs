using System;
using System.Threading.Tasks;
using Testcontainers.MongoDb;
using Xunit;

namespace MyCompanyName.MyProjectName.MongoDB;

public class MyProjectNameMongoDbFixture : IAsyncLifetime
{
    private readonly static MongoDbContainer _mongoDbContainer = new MongoDbBuilder().WithCommand().Build();

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public static string GetRandomConnectionString()
    {
        var randomDbName = "Db_" + Guid.NewGuid().ToString("N");
        return _mongoDbContainer.GetConnectionString().EnsureEndsWith('/') + randomDbName + "?authSource=admin";
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync().AsTask();
    }
}
