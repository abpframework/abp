using System;

namespace Volo.Abp.EntityFrameworkCore;

public static class EfCoreDatabaseProviderHelper
{
    public static EfCoreDatabaseProvider? GetDatabaseProviderOrNull(string? providerName)
    {
        if (providerName.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (providerName.Contains("SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.SqlServer;
        }
        if (providerName.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) ||
            providerName.Contains("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.PostgreSql;
        }
        if (providerName.Contains("MySql", StringComparison.OrdinalIgnoreCase) ||
            providerName.Contains("Pomelo", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.MySql;
        }
        if (providerName.Contains("Oracle", StringComparison.OrdinalIgnoreCase) ||
            providerName.Contains("Devart", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.Oracle;
        }
        if (providerName.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.Sqlite;
        }
        if (providerName.Contains("InMemory", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.InMemory;
        }
        if (providerName.Contains("Firebird", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.Firebird;
        }
        if (providerName.Contains("Cosmos", StringComparison.OrdinalIgnoreCase))
        {
            return EfCoreDatabaseProvider.Cosmos;
        }

        return null;
    }
}
