using JetBrains.Annotations;
using System;
using Devart.Data.Oracle.Entity;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsOracleDevartExtensions
    {
        public static void UseOracle(
                [NotNull] this AbpDbContextOptions options,
                [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UseOracle(oracleOptionsAction);
            });
        }

        public static void UseOracle<TDbContext>(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            options.Configure<TDbContext>(context =>
            {
                context.UseOracle(oracleOptionsAction);
            });
        }
    }
}
