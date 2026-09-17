using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.MySQL;

namespace Volo.Abp.EntityFrameworkCore;

public static class AbpDbContextConfigurationContextMySQLExtensions
{
    public static DbContextOptionsBuilder UseMySQL(
        [NotNull] this AbpDbContextConfigurationContext context,
        Action<MySql.EntityFrameworkCore.Infrastructure.MySQLDbContextOptionsBuilder>? mySQLOptionsAction = null)
    {
        var dbContextOptionsBuilder = context.ExistingConnection != null
            ? context.DbContextOptions.UseMySQL(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                mySQLOptionsAction?.Invoke(optionsBuilder);
            })
            : context.DbContextOptions.UseMySQL(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                mySQLOptionsAction?.Invoke(optionsBuilder);
            });

        ((IDbContextOptionsBuilderInfrastructure)dbContextOptionsBuilder)
            .AddOrUpdateExtension(new AbpMySQLDbContextOptionsExtension());

        return dbContextOptionsBuilder;
    }
}
