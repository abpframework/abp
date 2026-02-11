using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore;

public static class AbpDbContextConfigurationContextMySQLExtensions
{
    public static DbContextOptionsBuilder UseMySQL(
        [NotNull] this AbpDbContextConfigurationContext context,
        Action<MySql.EntityFrameworkCore.Infrastructure.MySQLDbContextOptionsBuilder>? mySQLOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseMySQL(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                mySQLOptionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseMySQL(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                mySQLOptionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}
