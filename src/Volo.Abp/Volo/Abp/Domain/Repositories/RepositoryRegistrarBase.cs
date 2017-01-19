using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

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
                    ? GetRepositoryTypeForDefaultPk(dbContextType, entityType)
                    : GetRepositoryType(dbContextType, entityType, primaryKeyType);
            }

            services.AddDefaultRepository(entityType, repositoryImplementationType);
        }

        protected abstract IEnumerable<Type> GetEntityTypes(Type dbContextType);

        protected abstract Type GetRepositoryTypeForDefaultPk(Type dbContextType, Type entityType);

        protected abstract Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType);
    }
}