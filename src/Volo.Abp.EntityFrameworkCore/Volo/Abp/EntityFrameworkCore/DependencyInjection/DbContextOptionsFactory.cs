using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.ExtensionMethods.Collections.Generic;

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
                var connInfoResolver = scope.ServiceProvider.GetRequiredService<IConnectionStringResolver>();

                var context = new AbpDbContextConfigurationContext<TDbContext>(connInfoResolver.Resolve(connectionStringName), connectionStringName);

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