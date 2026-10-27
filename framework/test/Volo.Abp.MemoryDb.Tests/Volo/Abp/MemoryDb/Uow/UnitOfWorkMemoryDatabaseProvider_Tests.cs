using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.MemoryDb.Uow;

public class UnitOfWorkMemoryDatabaseProvider_Tests : MemoryDbTestBase
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly IMemoryDatabaseProvider<TestAppMemoryDbContext> _memoryDatabaseProvider;

    public UnitOfWorkMemoryDatabaseProvider_Tests()
    {
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _connectionStringResolver = GetRequiredService<IConnectionStringResolver>();
        _memoryDatabaseProvider = GetRequiredService<IMemoryDatabaseProvider<TestAppMemoryDbContext>>();
    }

    [Fact]
    public async Task Should_Not_Use_Connection_String_As_Database_Api_Key()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync<TestAppMemoryDbContext>();

        using (var uow = _unitOfWorkManager.Begin())
        {
            await _memoryDatabaseProvider.GetDatabaseAsync();

            uow.FindDatabaseApi($"{typeof(TestAppMemoryDbContext).FullName}_{connectionString}").ShouldBeNull();
            uow.FindDatabaseApi($"{typeof(TestAppMemoryDbContext).FullName}_{connectionString.ToSha256()}").ShouldNotBeNull();

            await uow.CompleteAsync();
        }
    }
}
