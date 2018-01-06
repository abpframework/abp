using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Reflection;

namespace Volo.Abp.DependencyInjection
{
    /// <summary>
    /// This is a base class for dbcoUse derived
    /// </summary>
    public abstract class CommonDbContextRegistrationOptions : ICommonDbContextRegistrationOptionsBuilder
    {
        public Type OriginalDbContextType { get; }

        public List<Type> ReplacedDbContextTypes { get; }

        public Type DefaultRepositoryDbContextType { get; protected set; }

        public Type DefaultRepositoryImplementationType { get; private set; }

        public Type DefaultRepositoryImplementationTypeWithDefaultPrimaryKey { get; private set; }

        public bool RegisterDefaultRepositories { get; private set; }

        public bool IncludeAllEntitiesForDefaultRepositories { get; private set; }

        public Dictionary<Type, Type> CustomRepositories { get; }

        public bool SpecifiedDefaultRepositoryTypes => DefaultRepositoryImplementationType != null && DefaultRepositoryImplementationTypeWithDefaultPrimaryKey != null;

        protected CommonDbContextRegistrationOptions(Type originalDbContextType)
        {
            OriginalDbContextType = originalDbContextType;
            DefaultRepositoryDbContextType = originalDbContextType;
            CustomRepositories = new Dictionary<Type, Type>();
            ReplacedDbContextTypes = new List<Type>();
        }

        public ICommonDbContextRegistrationOptionsBuilder ReplaceDbContext<TOtherDbContext>()
        {
            return ReplaceDbContext(typeof(TOtherDbContext));
        }

        public ICommonDbContextRegistrationOptionsBuilder ReplaceDbContext(Type otherDbContextType)
        {
            if (!otherDbContextType.IsAssignableFrom(OriginalDbContextType))
            {
                throw new AbpException($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {otherDbContextType.AssemblyQualifiedName}!");
            }

            ReplacedDbContextTypes.Add(otherDbContextType);

            return this;
        }

        public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(bool includeAllEntities = false)
        {
            RegisterDefaultRepositories = true;
            IncludeAllEntitiesForDefaultRepositories = includeAllEntities;

            return this;
        }

        public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(bool includeAllEntities = false)
        {
            return AddDefaultRepositories(typeof(TDefaultRepositoryDbContext), includeAllEntities);
        }

        public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(Type defaultRepositoryDbContextType, bool includeAllEntities = false)
        {
            if (!defaultRepositoryDbContextType.IsAssignableFrom(OriginalDbContextType))
            {
                throw new AbpException($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {defaultRepositoryDbContextType.AssemblyQualifiedName}!");
            }

            DefaultRepositoryDbContextType = defaultRepositoryDbContextType;

            return this;
        }

        public ICommonDbContextRegistrationOptionsBuilder AddCustomRepository<TEntity, TRepository>()
        {
            WithCustomRepository(typeof(TEntity), typeof(TRepository));

            return this;
        }

        public ICommonDbContextRegistrationOptionsBuilder SetDefaultRepositoryClasses([NotNull] Type repositoryImplementationType, [NotNull] Type repositoryImplementationTypeWithDefaultPrimaryKey)
        {
            Check.NotNull(repositoryImplementationType, nameof(repositoryImplementationType));
            Check.NotNull(repositoryImplementationTypeWithDefaultPrimaryKey, nameof(repositoryImplementationTypeWithDefaultPrimaryKey));

            DefaultRepositoryImplementationType = repositoryImplementationType;
            DefaultRepositoryImplementationTypeWithDefaultPrimaryKey = repositoryImplementationTypeWithDefaultPrimaryKey;

            return this;
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

            CustomRepositories[entityType] = repositoryType;
        }

        public bool ShouldRegisterDefaultRepositoryFor(Type entityType)
        {
            if (!RegisterDefaultRepositories)
            {
                return false;
            }

            if (CustomRepositories.ContainsKey(entityType))
            {
                return false;
            }

            if (!IncludeAllEntitiesForDefaultRepositories && !ReflectionHelper.IsAssignableToGenericType(entityType, typeof(IAggregateRoot<>)))
            {
                return false;
            }

            return true;
        }
    }
}