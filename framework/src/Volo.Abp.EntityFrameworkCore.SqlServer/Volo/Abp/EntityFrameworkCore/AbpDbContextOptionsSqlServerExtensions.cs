using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsSqlServerExtensions
    {
        public static void UseSqlServer(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UseSqlServer(sqlServerOptionsAction);
            });
        }

        public static void UseSqlServer<TDbContext>(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            options.Configure<TDbContext>(context =>
            {
                context.UseSqlServer(sqlServerOptionsAction);
            });
        }
    }
}