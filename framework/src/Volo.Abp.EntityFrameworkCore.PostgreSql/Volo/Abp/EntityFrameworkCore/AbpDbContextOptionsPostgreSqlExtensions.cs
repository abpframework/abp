using System;
using JetBrains.Annotations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsPostgreSqlExtensions
    {
        public static void UsePostgreSql(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> postgreSqlOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UsePostgreSql(postgreSqlOptionsAction);
            });
        }

        public static void UsePostgreSql<TDbContext>(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> postgreSqlOptionsAction = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            options.Configure<TDbContext>(context =>
            {
                context.UsePostgreSql(postgreSqlOptionsAction);
            });
        }
    }
}