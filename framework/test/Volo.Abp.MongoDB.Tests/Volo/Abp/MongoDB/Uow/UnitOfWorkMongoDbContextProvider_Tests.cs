using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.TestApp.MongoDB;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.MongoDB.Uow;

[Collection(MongoTestCollection.Name)]
public class UnitOfWorkMongoDbContextProvider_Tests : MongoDbTestBase
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly IMongoDbContextProvider<TestAppMongoDbContext> _mongoDbContextProvider;

    public UnitOfWorkMongoDbContextProvider_Tests()
    {
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _connectionStringResolver = GetRequiredService<IConnectionStringResolver>();
        _mongoDbContextProvider = GetRequiredService<IMongoDbContextProvider<TestAppMongoDbContext>>();
    }

    [Fact]
    public async Task Should_Not_Use_Connection_String_As_Database_Api_Key()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync<TestAppMongoDbContext>();

        using (var uow = _unitOfWorkManager.Begin())
        {
            await _mongoDbContextProvider.GetDbContextAsync();

            uow.FindDatabaseApi($"{typeof(TestAppMongoDbContext).FullName}_{connectionString}").ShouldBeNull();
            uow.FindDatabaseApi($"{typeof(TestAppMongoDbContext).FullName}_{connectionString.ToSha256()}").ShouldNotBeNull();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Should_Not_Use_Connection_String_As_Transaction_Api_Key()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync<TestAppMongoDbContext>();
        var mongoUrl = new MongoUrl(connectionString);

        using (var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            await _mongoDbContextProvider.GetDbContextAsync();

            uow.FindTransactionApi($"MongoDb_{mongoUrl}").ShouldBeNull();
            uow.FindTransactionApi($"MongoDb_{mongoUrl.ToString().ToSha256()}").ShouldNotBeNull();

            await uow.CompleteAsync();
        }
    }
}
