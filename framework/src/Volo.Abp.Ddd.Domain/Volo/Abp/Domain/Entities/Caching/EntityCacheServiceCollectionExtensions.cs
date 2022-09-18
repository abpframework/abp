using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Volo.Abp.Domain.Entities.Caching;

public static class EntityCacheServiceCollectionExtensions
{
    public static IServiceCollection AddEntityCache<TEntity, TKey>(
        this IServiceCollection services) 
        where TEntity : Entity<TKey>
    {
        services
            .TryAddTransient<
                IEntityCache<TEntity, TKey>,
                EntityCacheWithoutCacheItem<TEntity, TKey>
            >();
        return services;
    }
    
    public static IServiceCollection AddEntityCache<TEntity, TEntityCacheItem, TKey>(
        this IServiceCollection services) 
        where TEntity : Entity<TKey>
        where TEntityCacheItem : class
    {
        services
            .TryAddTransient<
                IEntityCache<TEntityCacheItem, TKey>,
                EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey>
            >();
        return services;
    }

    public static IServiceCollection AddEntityCache<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>(
        this IServiceCollection services) 
        where TEntity : Entity<TKey>
        where TEntityCacheItem : class
    {
        services
            .TryAddTransient<
                IEntityCache<TEntityCacheItem, TKey>,
                EntityCacheWithObjectMapperContext<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>
            >();
        return services;
    }
}