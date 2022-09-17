using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Domain.Entities.Caching;

public static class EntityCacheServiceCollectionExtensions
{
    public static IServiceCollection AddEntityCache<TEntity, TKey>(
        this IServiceCollection services) 
        where TEntity : Entity<TKey>
    {
        return services
            .AddTransient<
                IEntityCache<TEntity, TKey>,
                EntityCacheWithoutCacheItem<TEntity, TKey>
            >();
    }
    
    public static IServiceCollection AddEntityCache<TEntity, TEntityCacheItem, TKey>(
        this IServiceCollection services) 
        where TEntity : Entity<TKey>
        where TEntityCacheItem : class
    {
        return services
            .AddTransient<
                IEntityCache<TEntityCacheItem, TKey>,
                EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey>
            >();
    }

    public static IServiceCollection AddEntityCache<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>(
        this IServiceCollection services) 
        where TEntity : Entity<TKey>
        where TEntityCacheItem : class
    {
        return services
            .AddTransient<
                IEntityCache<TEntityCacheItem, TKey>,
                EntityCacheWithObjectMapperContext<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>
            >();
    }
}