﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpMongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbContext<TMongoDbContext>(this IServiceCollection services, Action<IAbpMongoDbContextRegistrationOptionsBuilder> optionsBuilder = null) //Created overload instead of default parameter
        where TMongoDbContext : AbpMongoDbContext
    {
        var options = new AbpMongoDbContextRegistrationOptions(typeof(TMongoDbContext), services);

        var replacedDbContextTypes = typeof(TMongoDbContext).GetCustomAttributes<ReplaceDbContextAttribute>(true)
            .SelectMany(x => x.ReplacedDbContextTypes).ToList();

        foreach (var dbContextType in replacedDbContextTypes)
        {
            options.ReplaceDbContext(dbContextType);
        }

        optionsBuilder?.Invoke(options);

        foreach (var entry in options.ReplacedDbContextTypes)
        {
            var originalDbContextType = entry.Key;
            var targetDbContextType = entry.Value ?? typeof(TMongoDbContext);

            services.Replace(
                ServiceDescriptor.Transient(
                    originalDbContextType,
                    sp => sp.GetRequiredService(targetDbContextType)
                )
            );

            services.Configure<AbpMongoDbContextOptions>(opts =>
            {
                opts.DbContextReplacements[originalDbContextType] = targetDbContextType;
            });
        }

        new MongoDbRepositoryRegistrar(options).AddRepositories();

        return services;
    }
}
