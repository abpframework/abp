using JetBrains.Annotations;
using System;

namespace Volo.Abp.EntityFrameworkCore;

public static class AbpDbContextOptionsMySQLExtensions
{
    public static void UseMySQL(
        [NotNull] this AbpDbContextOptions options,
        Action<MySql.EntityFrameworkCore.Infrastructure.MySQLDbContextOptionsBuilder>? mySQLOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseMySQL(mySQLOptionsAction);
        });
    }

    public static void UseMySQL<TDbContext>(
        [NotNull] this AbpDbContextOptions options,
        Action<MySql.EntityFrameworkCore.Infrastructure.MySQLDbContextOptionsBuilder>? mySQLOptionsAction = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseMySQL(mySQLOptionsAction);
        });
    }
}
