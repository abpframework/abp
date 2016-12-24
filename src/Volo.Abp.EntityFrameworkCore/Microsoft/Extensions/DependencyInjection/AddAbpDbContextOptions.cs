using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AddAbpDbContextOptions
    {
        internal RepositoryRegistrationOptions Repositories { get; set; }

        public AddAbpDbContextOptions()
        {
            Repositories = new RepositoryRegistrationOptions();
        }

        /// <summary>
        /// Registers default repositories for this DbContext. 
        /// </summary>
        /// <param name="includeAllEntities">
        /// Registers repositories only for aggregate root entities by default.
        /// set <see cref="includeAllEntities"/> to true to include all entities.
        /// </param>
        public void AddDefaultRepositories(bool includeAllEntities = false)
        {
            Repositories.RegisterDefaultRepositories = true;
            Repositories.IncludeAllEntitiesForDefaultRepositories = includeAllEntities;
        }

        public void AddCustomRepository<TEntity, TRepository>()
        {
            AddCustomRepository(typeof(TEntity), typeof(TRepository));
        }

        private void AddCustomRepository(Type entityType, Type repositoryType)
        {
            if (!ReflectionHelper.IsAssignableToGenericType(entityType, typeof(IEntity<>)))
            {
                throw new AbpException($"Given entityType is not an entity: {entityType.AssemblyQualifiedName}. It must implement {typeof(IEntity<>).AssemblyQualifiedName}.");
            }

            if (!ReflectionHelper.IsAssignableToGenericType(repositoryType, typeof(IRepository<,>)))
            {
                throw new AbpException($"Given repositoryType is not a repository: {entityType.AssemblyQualifiedName}. It must implement {typeof(IRepository<,>).AssemblyQualifiedName}.");
            }

            Repositories.CustomRepositories[entityType] = repositoryType;
        }
    }
}