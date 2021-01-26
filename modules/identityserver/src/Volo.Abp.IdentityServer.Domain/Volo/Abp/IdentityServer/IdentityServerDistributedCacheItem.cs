using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.IdentityServer
{
    [Serializable]
    [IgnoreMultiTenancy]
    public class IdentityServerDistributedCacheItem<T>
        where T : class
    {
        public T Value { get; set; }

        public IdentityServerDistributedCacheItem()
        {

        }

        public IdentityServerDistributedCacheItem(T value)
        {
            Value = value;
        }
    }
}
