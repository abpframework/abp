using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextConfigurationContextPostgreSqlExtensions
    {
        public static DbContextOptionsBuilder UsePostgreSql(
            [NotNull] this AbpDbContextConfigurationContext context,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> sqlServerOptionsAction = null)
        {
            if (context.ExistingConnection != null)
            {
                return context.DbContextOptions.UseNpgsql(context.ExistingConnection, sqlServerOptionsAction);
            }
            else
            {
                return context.DbContextOptions.UseNpgsql(context.ConnectionString, sqlServerOptionsAction);
            }
        }
    }
}
