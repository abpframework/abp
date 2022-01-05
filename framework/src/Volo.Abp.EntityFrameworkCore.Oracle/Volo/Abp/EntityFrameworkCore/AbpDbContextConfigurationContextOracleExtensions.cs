using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Oracle.EntityFrameworkCore.Infrastructure;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore;

public static class AbpDbContextConfigurationContextOracleExtensions
{
    public static DbContextOptionsBuilder UseOracle(
       [NotNull] this AbpDbContextConfigurationContext context,
       [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseOracle(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                oracleOptionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseOracle(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                oracleOptionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}
