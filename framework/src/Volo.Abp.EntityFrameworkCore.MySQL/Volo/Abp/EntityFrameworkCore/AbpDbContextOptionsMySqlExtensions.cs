using JetBrains.Annotations;
using MySql.Data.EntityFrameworkCore.Infraestructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsMySqlExtensions
    {
        public static void UseMySql(
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
