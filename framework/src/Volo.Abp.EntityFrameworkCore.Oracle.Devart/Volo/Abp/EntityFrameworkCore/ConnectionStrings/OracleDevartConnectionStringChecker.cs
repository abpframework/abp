using System;
using System.Threading.Tasks;
using Devart.Data.Oracle;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.ConnectionStrings;

[Dependency(ReplaceServices = true)]
public class OracleDevartConnectionStringChecker : IConnectionStringChecker, ITransientDependency
{
    public virtual async Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new AbpConnectionStringCheckResult();
        var connString = new OracleConnectionStringBuilder(connectionString)
        {
            ConnectionTimeout = 1
        };

        try
        {
            await using var conn = new OracleConnection(connString.ConnectionString);
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
