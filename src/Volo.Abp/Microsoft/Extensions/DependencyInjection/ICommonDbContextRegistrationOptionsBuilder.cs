namespace Microsoft.Extensions.DependencyInjection
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
        void WithDefaultRepositories(bool includeAllEntities = false);

        /// <summary>
        /// Registers custom repository for a specific entity.
        /// Custom repositories overrides default repositories.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TRepository">Repository type</typeparam>
        void WithCustomRepository<TEntity, TRepository>();
    }
}