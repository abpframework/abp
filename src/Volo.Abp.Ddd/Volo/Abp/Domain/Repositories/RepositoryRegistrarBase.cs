using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Reflection;

namespace Volo.Abp.Domain.Repositories
{
    public abstract class RepositoryRegistrarBase<TOptions>
        where TOptions: CommonDbContextRegistrationOptions
    {
        public TOptions Options { get; }

        protected RepositoryRegistrarBase(TOptions options)
        {
            Options = options;
        }

        public virtual void AddRepositories(IServiceCollection services)
        {
            foreach (var customRepository in Options.CustomRepositories)
            {
                services.AddDefaultRepository(customRepository.Key, customRepository.Value);
            }

            if (Options.RegisterDefaultRepositories)
            {
                RegisterDefaultRepositories(services);
            }
        }

        protected virtual void RegisterDefaultRepositories(IServiceCollection services)
        {
            foreach (var entityType in GetEntityTypes(Options.DefaultRepositoryDbContextType))
            {
                if (!ShouldRegisterDefaultRepositoryFor(entityType))
                {
                    continue;
                }

                RegisterDefaultRepository(services, entityType);
            }
        }

        protected void RegisterDefaultRepository(IServiceCollection services, Type entityType)
        {
            var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityType);
            var isDefaultPrimaryKey = primaryKeyType == typeof(Guid);

            Type repositoryImplementationType;
            if (Options.SpecifiedDefaultRepositoryTypes)
            {
                repositoryImplementationType = isDefaultPrimaryKey
                    ? Options.DefaultRepositoryImplementationTypeWithDefaultPrimaryKey.MakeGenericType(entityType)
                    : Options.DefaultRepositoryImplementationType.MakeGenericType(entityType, primaryKeyType);
            }
            else
            {
                repositoryImplementationType = isDefaultPrimaryKey
                    ? GetRepositoryTypeForDefaultPk(Options.DefaultRepositoryDbContextType, entityType)
                    : GetRepositoryType(Options.DefaultRepositoryDbContextType, entityType, primaryKeyType);
            }

            services.AddDefaultRepository(entityType, repositoryImplementationType);
        }

        public bool ShouldRegisterDefaultRepositoryFor(Type entityType)
        {
            if (!Options.RegisterDefaultRepositories)
            {
                return false;
            }

            if (Options.CustomRepositories.ContainsKey(entityType))
            {
                return false;
            }

            if (!Options.IncludeAllEntitiesForDefaultRepositories && !ReflectionHelper.IsAssignableToGenericType(entityType, typeof(IAggregateRoot<>)))
            {
                return false;
            }

            return true;
        }

        protected abstract IEnumerable<Type> GetEntityTypes(Type dbContextType);

        protected abstract Type GetRepositoryTypeForDefaultPk(Type dbContextType, Type entityType);

        protected abstract Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType);
    }
}