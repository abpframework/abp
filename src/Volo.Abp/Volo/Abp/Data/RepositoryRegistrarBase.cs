using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Data
{
    public abstract class RepositoryRegistrarBase<TOptions>
        where TOptions: CommonDbContextRegistrationOptions
    {
        public TOptions Options { get; }

        protected RepositoryRegistrarBase(TOptions options)
        {
            Options = options;
        }

        public virtual void AddRepositories(IServiceCollection services, Type dbContextType)
        {
            foreach (var customRepository in Options.CustomRepositories)
            {
                services.AddDefaultRepository(customRepository.Key, customRepository.Value);
            }

            if (Options.RegisterDefaultRepositories)
            {
                RegisterDefaultRepositories(services, dbContextType);
            }
        }

        protected virtual void RegisterDefaultRepositories(IServiceCollection services, Type dbContextType)
        {
            foreach (var entityType in GetEntityTypes(dbContextType))
            {
                if (!Options.ShouldRegisterDefaultRepositoryFor(entityType))
                {
                    continue;
                }

                RegisterDefaultRepository(services, dbContextType, entityType);
            }
        }

        protected void RegisterDefaultRepository(IServiceCollection services, Type dbContextType, Type entityType)
        {
            var repositoryImplementationType = typeof(IEntity).GetTypeInfo().IsAssignableFrom(entityType)
                ? GetRepositoryTypeForDefaultPk(dbContextType, entityType)
                : GetRepositoryType(dbContextType, entityType, EntityHelper.GetPrimaryKeyType(entityType));

            services.AddDefaultRepository(entityType, repositoryImplementationType);
        }

        public abstract IEnumerable<Type> GetEntityTypes(Type dbContextType);

        protected abstract Type GetRepositoryTypeForDefaultPk(Type dbContextType, Type entityType);

        protected abstract Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType);
    }
}