using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Devart.Data.Oracle.Entity;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore;

public static class AbpDbContextConfigurationContextOracleDevartExtensions
{
    public static DbContextOptionsBuilder UseOracle(
       [NotNull] this AbpDbContextConfigurationContext context,
       [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null,
       bool useExistingConnectionIfAvailable = false)
    {
        if (useExistingConnectionIfAvailable && context.ExistingConnection != null)
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
