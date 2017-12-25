using System;

namespace Volo.Abp.DependencyInjection
{
    public interface ICommonDbContextRegistrationOptionsBuilder
    {
        /// <summary>
        /// Registers default repositories for this DbContext. 
        /// </summary>
        /// <param name="includeAllEntities">
        /// Registers repositories only for aggregate root entities by default.
        /// set <see cref="includeAllEntities"/> to true to include all entities.
        /// </param>
        ICommonDbContextRegistrationOptionsBuilder WithDefaultRepositories(bool includeAllEntities = false);

        /// <summary>
        /// Registers custom repository for a specific entity.
        /// Custom repositories overrides default repositories.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TRepository">Repository type</typeparam>
        ICommonDbContextRegistrationOptionsBuilder WithCustomRepository<TEntity, TRepository>();

        /// <summary>
        /// Uses given class(es) for default repositories.
        /// </summary>
        /// <param name="repositoryImplementationType">Repository implementation type</param>
        /// <param name="repositoryImplementationTypeWithDefaultPrimaryKey">Repository implementation type for default primary key type (<see cref="string"/>)</param>
        /// <returns></returns>
        ICommonDbContextRegistrationOptionsBuilder WithDefaultRepositoryClasses(Type repositoryImplementationType, Type repositoryImplementationTypeWithDefaultPrimaryKey);
    }
}