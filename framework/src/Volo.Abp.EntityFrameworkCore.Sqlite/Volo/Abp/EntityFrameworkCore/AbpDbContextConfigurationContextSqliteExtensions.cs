using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextConfigurationContextSqliteExtensions
    {
        public static DbContextOptionsBuilder UseSqlite(
            [NotNull] this AbpDbContextConfigurationContext context,
            [CanBeNull] Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null)
        {
            if (context.ExistingConnection != null)
            {
                return context.DbContextOptions.UseSqlite(context.ExistingConnection, sqliteOptionsAction);
            }
            else
            {
                return context.DbContextOptions.UseSqlite(context.ConnectionString, sqliteOptionsAction);
            }
        }
    }
}
