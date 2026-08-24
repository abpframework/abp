using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Uow;

public class UnitOfWorkDbContextProvider_Tests : EntityFrameworkCoreTestBase
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;

    public UnitOfWorkDbContextProvider_Tests()
    {
        _personRepository = GetRequiredService<IPersonRepository>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _connectionStringResolver = GetRequiredService<IConnectionStringResolver>();
    }

    [Fact]
    public async Task Should_Not_Use_Connection_String_As_Database_Api_Key()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync(
            ConnectionStringNameAttribute.GetConnStringName<TestAppDbContext>()
        );

        using (var uow = _unitOfWorkManager.Begin())
        {
            await _personRepository.GetDbContextAsync();

            uow.FindDatabaseApi($"{typeof(TestAppDbContext).FullName}_{connectionString}").ShouldBeNull();
            uow.FindDatabaseApi($"{typeof(TestAppDbContext).FullName}_{connectionString.ToSha256()}").ShouldNotBeNull();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Should_Not_Use_Connection_String_As_Transaction_Api_Key()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync(
            ConnectionStringNameAttribute.GetConnStringName<TestAppDbContext>()
        );

        using (var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            await _personRepository.GetDbContextAsync();

            uow.FindTransactionApi($"EntityFrameworkCore_{connectionString}").ShouldBeNull();
            uow.FindTransactionApi($"EntityFrameworkCore_{connectionString.ToSha256()}").ShouldNotBeNull();

            await uow.CompleteAsync();
        }
    }
}
