using System;
using JetBrains.Annotations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsSqlServerExtensions
    {
        public static void UsePostgreSql(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> sqlServerOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UsePostgreSql(sqlServerOptionsAction);
            });
        }

        public static void UsePostgreSql<TDbContext>(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> sqlServerOptionsAction = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            options.Configure<TDbContext>(context =>
            {
                context.UsePostgreSql(sqlServerOptionsAction);
            });
        }
    }
}