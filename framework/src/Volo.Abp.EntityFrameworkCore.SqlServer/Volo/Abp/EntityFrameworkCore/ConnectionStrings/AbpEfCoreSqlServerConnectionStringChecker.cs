using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

[ExposeServices(typeof(IAbpConnectionStringChecker))]
public class AbpEfCoreSqlServerConnectionStringChecker : IAbpConnectionStringChecker, ITransientDependency
{
    public virtual async Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new AbpConnectionStringCheckResult();
        var connString = new SqlConnectionStringBuilder(connectionString)
        {
            ConnectTimeout = 1
        };

        var oldDatabaseName = connString.InitialCatalog;
        connString.InitialCatalog = "master";

        try
        {
            await using var conn = new SqlConnection(connString.ConnectionString);
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
