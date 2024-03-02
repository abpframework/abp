using System;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore;

public class MyProjectNameEntityFrameworkCoreFixture : IAsyncLifetime
{
    private readonly static MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public static string GetRandomConnectionString()
    {
        var randomDbName = "Database=Db_" + Guid.NewGuid().ToString("N");
        return _msSqlContainer.GetConnectionString().Replace("Database=master", randomDbName, StringComparison.OrdinalIgnoreCase);
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync().AsTask();
    }
}
