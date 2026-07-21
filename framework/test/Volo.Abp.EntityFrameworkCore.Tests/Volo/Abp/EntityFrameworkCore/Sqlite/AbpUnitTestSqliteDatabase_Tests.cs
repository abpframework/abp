using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Sqlite;

public class AbpUnitTestSqliteDatabase_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
{
    [Fact]
    public async Task Non_Transactional_DbContexts_Should_Use_Separate_Connections_To_The_Same_Database()
    {
        DbConnection firstConnection = null;
        DbConnection secondConnection = null;

        await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions { IsTransactional = false }, async () =>
        {
            firstConnection = (await GetTestAppDbContextAsync()).Database.GetDbConnection();
        });

        await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions { IsTransactional = false }, async () =>
        {
            secondConnection = (await GetTestAppDbContextAsync()).Database.GetDbConnection();
        });

        firstConnection.ShouldNotBeSameAs(secondConnection);
        firstConnection.ConnectionString.ShouldBe(secondConnection.ConnectionString);
    }

    [Fact]
    public async Task Transactional_Uow_Should_Share_One_Connection_And_Transaction_Across_DbContexts()
    {
        await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions { IsTransactional = true }, async () =>
        {
            var firstDbContext = await GetTestAppDbContextAsync();
            var secondDbContext = await ServiceProvider
                .GetRequiredService<IDbContextProvider<SecondDbContext>>()
                .GetDbContextAsync();

            firstDbContext.Database.GetDbConnection().ShouldBeSameAs(secondDbContext.Database.GetDbConnection());

            firstDbContext.Database.CurrentTransaction.ShouldNotBeNull();
            secondDbContext.Database.CurrentTransaction.ShouldNotBeNull();
            firstDbContext.Database.CurrentTransaction.GetDbTransaction()
                .ShouldBeSameAs(secondDbContext.Database.CurrentTransaction.GetDbTransaction());
        });
    }

    [Fact]
    public async Task Concurrent_DbContext_Initialization_Should_Not_Race_On_Sqlite_Function_Registration()
    {
        var tasks = new List<Task>();

        for (var i = 0; i < 12; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                for (var round = 0; round < 20; round++)
                {
                    await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions { IsTransactional = false }, async () =>
                    {
                        var repository = ServiceProvider.GetRequiredService<IReadOnlyRepository<Person, Guid>>();
                        await repository.GetListAsync();
                    });
                }
            }));
        }

        // A shared connection object would throw "unable to delete/modify user-function due to active statements".
        await Task.WhenAll(tasks);
    }

    [Fact]
    public void Named_Database_Should_Live_And_Die_With_The_Keeper_Connection()
    {
        var database = new AbpUnitTestSqliteDatabase();
        var connectionString = database.ConnectionString;

        connectionString.ShouldContain("Mode=Memory");
        connectionString.ShouldContain("Cache=Shared");

        using (var writer = new AbpUnitTestSqliteConnection(connectionString))
        {
            writer.Open();
            using var command = writer.CreateCommand();
            command.CommandText = "CREATE TABLE KeeperProbe (Id INTEGER PRIMARY KEY);";
            command.ExecuteNonQuery();
        }

        using (var reader = new AbpUnitTestSqliteConnection(connectionString))
        {
            reader.Open();
            using var command = reader.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM KeeperProbe;";
            Convert.ToInt64(command.ExecuteScalar()).ShouldBe(0L);
        }

        database.Dispose();

        using (var afterDispose = new AbpUnitTestSqliteConnection(connectionString))
        {
            afterDispose.Open();
            using var command = afterDispose.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM KeeperProbe;";
            Should.Throw<SqliteException>(() => command.ExecuteScalar());
        }
    }

    private async Task<TestAppDbContext> GetTestAppDbContextAsync()
    {
        return await ServiceProvider
            .GetRequiredService<IDbContextProvider<TestAppDbContext>>()
            .GetDbContextAsync();
    }
}
