namespace Volo.Abp.Domain.Entities.Caching;

public class EntityCacheItemWrapper<TEntityCacheItem>
    where TEntityCacheItem : class
{
    public TEntityCacheItem? Value { get; set; }

    public EntityCacheItemWrapper(TEntityCacheItem? value)
    {
        Value = value;
    }
}
