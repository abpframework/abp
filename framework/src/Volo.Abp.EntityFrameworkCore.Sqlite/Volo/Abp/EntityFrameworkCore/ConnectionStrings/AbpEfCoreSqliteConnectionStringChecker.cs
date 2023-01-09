using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

[ExposeServices(typeof(IAbpConnectionStringChecker))]
public class AbpEfCoreSqliteConnectionStringChecker : IAbpConnectionStringChecker, ITransientDependency
{
    public virtual async Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new AbpConnectionStringCheckResult();

        try
        {
            await using var conn = new SqliteConnection(connectionString);
            await conn.OpenAsync();
            result.Connected = true;
            result.DatabaseExists = true;

            await conn.CloseAsync();

            return result;
        }
        catch (Exception e)
        {
            return result;
        }
    }
}
