using System;
using System.Reflection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbContext<TMongoDbContext>(this IServiceCollection services, Action<IMongoDbContextRegistrationOptionsBuilder> optionsBuilder = null) //Created overload instead of default parameter
            where TMongoDbContext : AbpMongoDbContext
        {
            var options = new MongoDbContextRegistrationOptions();
            optionsBuilder?.Invoke(options);

            AddRepositories<TMongoDbContext>(services, options);

            return services;
        }

        private static void AddRepositories<TMongoDbContext>(IServiceCollection services, MongoDbContextRegistrationOptions options)
            where TMongoDbContext : AbpMongoDbContext
        {
            foreach (var customRepository in options.CustomRepositories)
            {
                services.AddDefaultRepository(customRepository.Key, customRepository.Value);
            }

            if (options.RegisterDefaultRepositories)
            {
                RegisterDefaultRepositories(services, typeof(TMongoDbContext), options);
            }
        }

        private static void RegisterDefaultRepositories(IServiceCollection services, Type dbContextType, MongoDbContextRegistrationOptions options)
        {
            var mongoDbContext = (AbpMongoDbContext) Activator.CreateInstance(dbContextType);

            foreach (var entityType in mongoDbContext.GetEntityCollectionTypes())
            {
                if (!options.ShouldRegisterDefaultRepositoryFor(entityType))
                {
                    continue;
                }

                RegisterDefaultRepository(services, dbContextType, entityType, options);
            }
        }

        private static void RegisterDefaultRepository(IServiceCollection services, Type dbContextType, Type entityType, MongoDbContextRegistrationOptions options)
        {
            var repositoryImplementationType = typeof(IEntity).GetTypeInfo().IsAssignableFrom(entityType)
                ? typeof(MongoDbRepository<,>).MakeGenericType(dbContextType, entityType)
                : typeof(MongoDbRepository<,,>).MakeGenericType(dbContextType, entityType, EntityHelper.GetPrimaryKeyType(entityType));

            services.AddDefaultRepository(entityType, repositoryImplementationType);
        }
    }
}
