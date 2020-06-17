using System;

namespace Volo.Abp.Uow
{
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
    }
}
