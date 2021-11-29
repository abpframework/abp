using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.DependencyInjection
{
    /// <summary>
    /// This is a base class for dbcoUse derived
    /// </summary>
    public abstract class AbpCommonDbContextRegistrationOptions : IAbpCommonDbContextRegistrationOptionsBuilder
    {
        public Type OriginalDbContextType { get; }

        public IServiceCollection Services { get; }

        public Dictionary<Type, Type> ReplacedDbContextTypes { get; }

        public Type DefaultRepositoryDbContextType { get; protected set; }

        public Type DefaultRepositoryImplementationType { get; private set; }

        public Type DefaultRepositoryImplementationTypeWithoutKey { get; private set; }

        public bool RegisterDefaultRepositories { get; private set; }

        public bool IncludeAllEntitiesForDefaultRepositories { get; private set; }

        public Dictionary<Type, Type> CustomRepositories { get; }

        public List<Type> SpecifiedDefaultRepositories { get; }

        public bool SpecifiedDefaultRepositoryTypes => DefaultRepositoryImplementationType != null && DefaultRepositoryImplementationTypeWithoutKey != null;

        protected AbpCommonDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
        {
            OriginalDbContextType = originalDbContextType;
            Services = services;
            DefaultRepositoryDbContextType = originalDbContextType;
            CustomRepositories = new Dictionary<Type, Type>();
            ReplacedDbContextTypes = new Dictionary<Type, Type>();
            SpecifiedDefaultRepositories = new List<Type>();
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder ReplaceDbContext<TOtherDbContext>()
        {
            return ReplaceDbContext(typeof(TOtherDbContext));
        }
        
        public IAbpCommonDbContextRegistrationOptionsBuilder ReplaceDbContext<TOtherDbContext, TTargetDbContext>()
        {
            return ReplaceDbContext(typeof(TOtherDbContext), typeof(TTargetDbContext));
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder ReplaceDbContext(Type otherDbContextType, Type targetDbContextType = null)
        {
            if (!otherDbContextType.IsAssignableFrom(OriginalDbContextType))
            {
                throw new AbpException($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {otherDbContextType.AssemblyQualifiedName}!");
            }

            ReplacedDbContextTypes[otherDbContextType] = targetDbContextType;

            return this;
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(bool includeAllEntities = false)
        {
            RegisterDefaultRepositories = true;
            IncludeAllEntitiesForDefaultRepositories = includeAllEntities;

            return this;
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(Type defaultRepositoryDbContextType, bool includeAllEntities = false)
        {
            if (!defaultRepositoryDbContextType.IsAssignableFrom(OriginalDbContextType))
            {
                throw new AbpException($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {defaultRepositoryDbContextType.AssemblyQualifiedName}!");
            }

            DefaultRepositoryDbContextType = defaultRepositoryDbContextType;

            return AddDefaultRepositories(includeAllEntities);
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(bool includeAllEntities = false)
        {
            return AddDefaultRepositories(typeof(TDefaultRepositoryDbContext), includeAllEntities);
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder AddDefaultRepository<TEntity>()
        {
            return AddDefaultRepository(typeof(TEntity));
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder AddDefaultRepository(Type entityType)
        {
            EntityHelper.CheckEntity(entityType);

            SpecifiedDefaultRepositories.AddIfNotContains(entityType);

            return this;
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder AddRepository<TEntity, TRepository>()
        {
            AddCustomRepository(typeof(TEntity), typeof(TRepository));

            return this;
        }

        public IAbpCommonDbContextRegistrationOptionsBuilder SetDefaultRepositoryClasses(
            Type repositoryImplementationType,
            Type repositoryImplementationTypeWithoutKey
            )
        {
            Check.NotNull(repositoryImplementationType, nameof(repositoryImplementationType));
            Check.NotNull(repositoryImplementationTypeWithoutKey, nameof(repositoryImplementationTypeWithoutKey));

            DefaultRepositoryImplementationType = repositoryImplementationType;
            DefaultRepositoryImplementationTypeWithoutKey = repositoryImplementationTypeWithoutKey;

            return this;
        }

        private void AddCustomRepository(Type entityType, Type repositoryType)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
            {
                throw new AbpException($"Given entityType is not an entity: {entityType.AssemblyQualifiedName}. It must implement {typeof(IEntity<>).AssemblyQualifiedName}.");
            }

            if (!typeof(IRepository).IsAssignableFrom(repositoryType))
            {
                throw new AbpException($"Given repositoryType is not a repository: {entityType.AssemblyQualifiedName}. It must implement {typeof(IBasicRepository<>).AssemblyQualifiedName}.");
            }

            CustomRepositories[entityType] = repositoryType;
        }
    }
}
