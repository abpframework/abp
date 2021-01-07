using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpDbContextOptionsProvider<TDbContext> : IDbContextOptionsProvider<TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        protected IServiceProvider ServiceProvider { get; }

        public AbpDbContextOptionsProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        [Obsolete("Use GetDbContextOptionsAsync method.")]
        public DbContextOptions<TDbContext> GetDbContextOptions()
        {
            var creationContext = GetCreationContext();

            var context = new AbpDbContextConfigurationContext<TDbContext>(
                creationContext.ConnectionString,
                ServiceProvider,
                creationContext.ConnectionStringName,
                creationContext.ExistingConnection
            );

            ConfigureDbContextOptions(context);

            return context.DbContextOptions.Options;
        }

        public async Task<DbContextOptions<TDbContext>> GetDbContextOptionsAsync()
        {
            var creationContext = await GetCreationContextAsync();

            var context = new AbpDbContextConfigurationContext<TDbContext>(
                creationContext.ConnectionString,
                ServiceProvider,
                creationContext.ConnectionStringName,
                creationContext.ExistingConnection
            );

            ConfigureDbContextOptions(context);

            return context.DbContextOptions.Options;
        }

        protected virtual void ConfigureDbContextOptions(AbpDbContextConfigurationContext<TDbContext> context)
        {
            var options = ServiceProvider.GetRequiredService<IOptions<AbpDbContextOptions>>().Value;

            PreConfigure(options, context);
            Configure(options, context);
        }

        protected virtual void PreConfigure(
            AbpDbContextOptions options,
            AbpDbContextConfigurationContext<TDbContext> context)
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

        protected virtual void Configure(
            AbpDbContextOptions options,
            AbpDbContextConfigurationContext<TDbContext> context)
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

        protected virtual DbContextCreationContext GetCreationContext()
        {
            var context = DbContextCreationContext.Current;
            if (context != null)
            {
                return context;
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();

            //Use DefaultConnectionStringResolver.Resolve when we remove IConnectionStringResolver.Resolve
            var connectionString = ServiceProvider.GetRequiredService<IConnectionStringResolver>().Resolve(connectionStringName);

            return new DbContextCreationContext(
                connectionStringName,
                connectionString
            );
        }

        protected virtual async Task<DbContextCreationContext> GetCreationContextAsync()
        {
            var context = DbContextCreationContext.Current;
            if (context != null)
            {
                return context;
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();

            //Use DefaultConnectionStringResolver.Resolve when we remove IConnectionStringResolver.Resolve
            var connectionString = await ServiceProvider.GetRequiredService<IConnectionStringResolver>().ResolveAsync(connectionStringName);

            return new DbContextCreationContext(
                connectionStringName,
                connectionString
            );
        }
    }
}
