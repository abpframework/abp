using System;

namespace Volo.Abp.Caching
{
    [Serializable]
    public class UnitOfWorkCacheItem : UnitOfWorkCacheItem<object>
    {

    }

    [Serializable]
    public class UnitOfWorkCacheItem<TValue>
        where TValue : class
    {
        public bool IsRemoved { get; set; }

        public TValue Value { get; set; }

        public UnitOfWorkCacheItem()
        {

        }

        public UnitOfWorkCacheItem(TValue value)
        {
            Value = value;
        }

        public UnitOfWorkCacheItem(TValue value, bool isRemoved)
        {
            Value = value;
            IsRemoved = isRemoved;
        }

        public UnitOfWorkCacheItem<TValue> SetValue(TValue value)
        {
            Value = value;
            IsRemoved = false;
            return this;
        }

        public UnitOfWorkCacheItem<TValue> RemoveValue()
        {
            Value = null;
            IsRemoved = true;
            return this;
        }
    }

    public static class UnitOfWorkCacheItemExtensions
    {
        public static object GetUnRemovedValue(this UnitOfWorkCacheItem item)
        {
            return item != null && !item.IsRemoved ? item.Value : null;
        }

        public static TValue GetUnRemovedValue<TValue>(this UnitOfWorkCacheItem<TValue> item)
            where TValue : class
        {
            return item != null && !item.IsRemoved ? item.Value : null;
        }
    }
}
