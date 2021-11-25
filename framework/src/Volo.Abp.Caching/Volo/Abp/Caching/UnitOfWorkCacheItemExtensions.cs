namespace Volo.Abp.Caching;

public static class UnitOfWorkCacheItemExtensions
{
    public static TValue GetUnRemovedValueOrNull<TValue>(this UnitOfWorkCacheItem<TValue> item)
        where TValue : class
    {
        return item != null && !item.IsRemoved ? item.Value : null;
    }
}
