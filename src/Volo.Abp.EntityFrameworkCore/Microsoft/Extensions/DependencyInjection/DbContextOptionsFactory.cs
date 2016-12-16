using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.ExtensionMethods.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DbContextOptionsFactory
    {
        public static DbContextOptions<TDbContext> Create<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : AbpDbContext<TDbContext>
        {
            const string moduleName = ""; //TODO: Use AbpModuleDescriptor instead of module name?

            using (var scope = serviceProvider.CreateScope())
            {
                var connInfoResolver = scope.ServiceProvider.GetRequiredService<IConnectionStringResolver>();

                var context = new AbpDbContextConfigurationContext<TDbContext>(connInfoResolver.Resolve(moduleName), moduleName);

                var dbContextOptions = scope.ServiceProvider.GetRequiredService<IOptions<AbpDbContextOptions>>().Value;

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
                    throw new AbpException("Should set a configure action for dbcontext"); //TODO: Better message
                }

                return context.DbContextOptions.Options;
            }
        }
    }
}