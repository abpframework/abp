using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AddAbpDbContextOptions
    {
        internal RepositoryRegistrationOptions RepositoryOptions { get; set; }

        public AddAbpDbContextOptions()
        {
            RepositoryOptions = new RepositoryRegistrationOptions();
        }

        /// <summary>
        /// Registers default repositories for this DbContext. 
        /// </summary>
        /// <param name="includeAllEntities">
        /// Registers repositories only for aggregate root entities by default.
        /// set <see cref="includeAllEntities"/> to true to include all entities.
        /// </param>
        public void WithDefaultRepositories(bool includeAllEntities = false)
        {
            RepositoryOptions.RegisterDefaultRepositories = true;
            RepositoryOptions.IncludeAllEntitiesForDefaultRepositories = includeAllEntities;
        }

        /// <summary>
        /// Registers custom repository for a specific entity.
        /// Custom repositories overrides default repositories.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TRepository">Repository type</typeparam>
        public void WithCustomRepository<TEntity, TRepository>()
        {
            WithCustomRepository(typeof(TEntity), typeof(TRepository));
        }

        private void WithCustomRepository(Type entityType, Type repositoryType)
        {
            if (!ReflectionHelper.IsAssignableToGenericType(entityType, typeof(IEntity<>)))
            {
                throw new AbpException($"Given entityType is not an entity: {entityType.AssemblyQualifiedName}. It must implement {typeof(IEntity<>).AssemblyQualifiedName}.");
            }

            if (!ReflectionHelper.IsAssignableToGenericType(repositoryType, typeof(IRepository<,>)))
            {
                throw new AbpException($"Given repositoryType is not a repository: {entityType.AssemblyQualifiedName}. It must implement {typeof(IRepository<,>).AssemblyQualifiedName}.");
            }

            RepositoryOptions.CustomRepositories[entityType] = repositoryType;
        }
    }
}