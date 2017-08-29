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
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = new AbpDbContextConfigurationContext<TDbContext>(
                    GetConnectionString(scope, connectionStringName),
                    connectionStringName,
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

        private static string GetConnectionString(IServiceScope scope, string connectionStringName)
        {
            return scope.ServiceProvider.GetRequiredService<IConnectionStringResolver>().Resolve(connectionStringName);
        }
    }
}