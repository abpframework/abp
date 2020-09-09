using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<TDbContext> Create<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var creationContext = GetCreationContext<TDbContext>(serviceProvider);

            var context = new AbpDbContextConfigurationContext<TDbContext>(
                creationContext.ConnectionString,
                serviceProvider,
                creationContext.ConnectionStringName,
                creationContext.ExistingConnection
            );

            var options = GetDbContextOptions<TDbContext>(serviceProvider);

            PreConfigure(options, context);
            Configure(options, context);

            return context.DbContextOptions.Options;
        }

        private static void PreConfigure<TDbContext>(
            AbpDbContextOptions options,
            AbpDbContextConfigurationContext<TDbContext> context)
            where TDbContext : AbpDbContext<TDbContext>
        {
            foreach (var defaultPreConfigureAction in options.DefaultPreConfigureActions)
            {
                defaultPreConfigureAction.Invoke(context);
            }

            var preConfigureActions = options.PreConfigureActions.GetOrDefault(typeof(TDbContext));
            if (!preConfigureActions.IsNullOrEmpty())
            {
                foreach (var preConfigureAction in preConfigureActions)
                {
                    ((Action<AbpDbContextConfigurationContext<TDbContext>>)preConfigureAction).Invoke(context);
                }
            }
        }

        private static void Configure<TDbContext>(
            AbpDbContextOptions options,
            AbpDbContextConfigurationContext<TDbContext> context)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var configureAction = options.ConfigureActions.GetOrDefault(typeof(TDbContext));
            if (configureAction != null)
            {
                ((Action<AbpDbContextConfigurationContext<TDbContext>>)configureAction).Invoke(context);
            }
            else if (options.DefaultConfigureAction != null)
            {
                options.DefaultConfigureAction.Invoke(context);
            }
            else
            {
                throw new AbpException(
                    $"No configuration found for {typeof(DbContext).AssemblyQualifiedName}! Use services.Configure<AbpDbContextOptions>(...) to configure it.");
            }
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
