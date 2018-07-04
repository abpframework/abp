using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Infraestructure;
using System;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextConfigurationContextMySQLExtensions
    {
        public static DbContextOptionsBuilder UseMySQL(
           [NotNull] this AbpDbContextConfigurationContext context,
           [CanBeNull] Action<MySQLDbContextOptionsBuilder> mySQLOptionsAction = null)
        {
            if (context.ExistingConnection != null)
            {
                return context.DbContextOptions.UseMySQL(context.ExistingConnection, mySQLOptionsAction);
            }
            else
            {
                return context.DbContextOptions.UseMySQL(context.ConnectionString, mySQLOptionsAction);
            }
        }
    }
}
