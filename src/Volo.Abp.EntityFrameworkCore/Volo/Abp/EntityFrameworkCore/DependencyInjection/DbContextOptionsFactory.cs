using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    internal static class DbContextOptionsFactory
    {
        public static DbContextOptions<TDbContext> Create<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var creationContext = GetCreationContext<TDbContext>(serviceProvider);

            var context = new AbpDbContextConfigurationContext<TDbContext>(
                creationContext.ConnectionString,
                creationContext.ConnectionStringName,
                serviceProvider
            );

            var dbContextOptions = GetDbContextOptions<TDbContext>(serviceProvider);

            var configureAction = dbContextOptions.ConfigureActions.GetOrDefault(typeof(TDbContext));
            if (configureAction != null)
            {
                ((Action<AbpDbContextConfigurationContext<TDbContext>>)configureAction).Invoke(context);
            }
            else if (dbContextOptions.DefaultConfigureAction != null)
            {
                dbContextOptions.DefaultConfigureAction.Invoke(context);
            }
            else
            {
                throw new AbpException($"No configuration found for {typeof(DbContext).AssemblyQualifiedName}! Use services.Configure<AbpDbContextOptions>(...) to configure it.");
            }

            return context.DbContextOptions.Options;
        }

        private static AbpDbContextOptions GetDbContextOptions<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : AbpDbContext<TDbContext>
        {
            return serviceProvider.GetRequiredService<IOptions<AbpDbContextOptions>>().Value;
        }

        private static DbContextCreationContext GetCreationContext<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var context = DbContextCreationContext.Current;
            if (context != null)
            {
                return context;
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = serviceProvider.GetRequiredService<IConnectionStringResolver>().Resolve(connectionStringName);

            return new DbContextCreationContext(
                connectionStringName,
                connectionString
            );
        }
    }
}