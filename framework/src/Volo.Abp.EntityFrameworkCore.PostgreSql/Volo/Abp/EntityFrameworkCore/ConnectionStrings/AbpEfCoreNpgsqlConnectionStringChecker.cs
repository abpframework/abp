using System;
using System.Threading.Tasks;
using Npgsql;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

[ExposeServices(typeof(IAbpConnectionStringChecker))]
public class AbpEfCoreNpgsqlConnectionStringChecker : IAbpConnectionStringChecker, ITransientDependency
{
    public virtual async Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new AbpConnectionStringCheckResult();
        var connString = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Timeout = 1
        };

        var oldDatabaseName = connString.Database;
        connString.Database = "postgres";

        try
        {
            await using var conn = new NpgsqlConnection(connString.ConnectionString);
            await conn.OpenAsync();
            result.Connected = true;
            await conn.ChangeDatabaseAsync(oldDatabaseName);
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
