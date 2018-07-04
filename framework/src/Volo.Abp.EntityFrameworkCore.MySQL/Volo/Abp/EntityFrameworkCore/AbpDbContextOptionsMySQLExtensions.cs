using JetBrains.Annotations;
using MySql.Data.EntityFrameworkCore.Infraestructure;
using System;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsMySQLExtensions
    {
        public static void UseMySQL(
                [NotNull] this AbpDbContextOptions options,
                [CanBeNull] Action<MySQLDbContextOptionsBuilder> mySQLOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UseMySQL(mySQLOptionsAction);
            });
        }

        public static void UseMySQL<TDbContext>(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<MySQLDbContextOptionsBuilder> mySQLOptionsAction = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            options.Configure<TDbContext>(context =>
            {
                context.UseMySQL(mySQLOptionsAction);
            });
        }
    }
}
