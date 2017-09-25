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
            using (var scope = serviceProvider.CreateScope())
            {
                var creationContext = GetCreationContext<TDbContext>(scope.ServiceProvider);

                var context = new AbpDbContextConfigurationContext<TDbContext>(
                    creationContext.ConnectionString,
                    creationContext.ConnectionStringName,
                    scope.ServiceProvider
                );

                var dbContextOptions = GetDbContextOptions<TDbContext>(scope);

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
        }

        private static AbpDbContextOptions GetDbContextOptions<TDbContext>(IServiceScope scope) where TDbContext : AbpDbContext<TDbContext>
        {
            return scope.ServiceProvider.GetRequiredService<IOptions<AbpDbContextOptions>>().Value;
        }

        private static DbContextOptionsFactoryContext GetCreationContext<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var context = DbContextOptionsFactoryContext.Current;
            if (context != null)
            {
                return context;
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = serviceProvider.GetRequiredService<IConnectionStringResolver>().Resolve(connectionStringName);

            return new DbContextOptionsFactoryContext(
                connectionStringName,
                connectionString
            );
        }
    }
}