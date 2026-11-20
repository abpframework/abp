using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.MongoDB.Clients;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.MongoDB;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Uow.MongoDB;
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

            uow.FindTransactionApi($"MongoDb_{mongoUrl.Url}").ShouldBeNull();
            uow.FindTransactionApi($"MongoDb_{mongoUrl.Url.ToSha256()}").ShouldNotBeNull();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public void Should_Use_Different_Transaction_Api_Keys_For_Different_Credentials()
    {
        var provider = new TestUnitOfWorkMongoDbContextProvider(
            _unitOfWorkManager,
            _connectionStringResolver,
            GetRequiredService<ICancellationTokenProvider>(),
            GetRequiredService<ICurrentTenant>(),
            GetRequiredService<IMongoDbContextTypeProvider>(),
            GetRequiredService<IAbpMongoClientFactory>());

        var key1 = provider.GetTransactionApiKeyPublic(new MongoUrl("mongodb://user1:pass1@localhost:27017/db"));
        var key2 = provider.GetTransactionApiKeyPublic(new MongoUrl("mongodb://user1:pass2@localhost:27017/db"));
        var key3 = provider.GetTransactionApiKeyPublic(new MongoUrl("mongodb://user1:pass1@localhost/db"));

        key1.ShouldNotBe(key2);
        key1.ShouldBe(key3);
    }

    private class TestUnitOfWorkMongoDbContextProvider : UnitOfWorkMongoDbContextProvider<TestAppMongoDbContext>
    {
        public TestUnitOfWorkMongoDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            ICancellationTokenProvider cancellationTokenProvider,
            ICurrentTenant currentTenant,
            IMongoDbContextTypeProvider dbContextTypeProvider,
            IAbpMongoClientFactory mongoClientFactory)
            : base(
                unitOfWorkManager,
                connectionStringResolver,
                cancellationTokenProvider,
                currentTenant,
                dbContextTypeProvider,
                mongoClientFactory)
        {
        }

        public string GetTransactionApiKeyPublic(MongoUrl url)
        {
            return GetTransactionApiKey(url);
        }
    }
}
