using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

[Dependency(ReplaceServices = true)]
public class MySQLConnectionStringChecker : IConnectionStringChecker, ITransientDependency
{
    public virtual async Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new AbpConnectionStringCheckResult();
        try
        {
            var connString = new MySqlConnectionStringBuilder(connectionString)
            {
                ConnectionLifeTime = 1
            };

            var oldDatabaseName = connString.Database;
            connString.Database = "mysql";

            await using var conn = new MySqlConnection(connString.ConnectionString);
            await conn.OpenAsync();
            result.Connected = true;
            await conn.ChangeDatabaseAsync(oldDatabaseName);
            result.DatabaseExists = true;

            await conn.CloseAsync();

            return result;
        }
        catch (Exception)
        {
            return result;
        }
    }
}
