using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public abstract class UnitOfWorkCache<TCacheItem> : IUnitOfWorkCache<TCacheItem>
        where TCacheItem : class
    {
        public abstract string Name { get; }

        protected IUnitOfWorkManager UnitOfWorkManager { get; }

        public UnitOfWorkCache(IUnitOfWorkManager unitOfWorkManager)
        {
            UnitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual async Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory)
        {
            var cache = GetUnitOfWorkCache();
            var value = cache.GetOrDefault(key);
            if (value != null && !value.IsRemoved)
            {
                return value.Value;
            }

            var item = new UnitOfWorkCacheItem<TCacheItem>(await factory());
            if (value == null)
            {
                cache.Add(key, item);
            }
            else
            {
                cache[key] = item;
            }

            return item.Value;
        }

        [UnitOfWork]
        public virtual Task<TCacheItem> GetOrNullAsync(string key)
        {
            var value = GetUnitOfWorkCache().GetOrDefault(key);
            return Task.FromResult(value != null && !value.IsRemoved ? value.Value: null);
        }

        [UnitOfWork]
        public virtual Task<TCacheItem> SetAsync(string key, TCacheItem item)
        {
            var cache = GetUnitOfWorkCache();
            if (cache.TryGetValue(key, out _))
            {
                cache[key].Value = item;
                cache[key].IsRemoved = false;
            }
            else
            {
                cache.Add(key, new UnitOfWorkCacheItem<TCacheItem>(item));
            }

            return Task.FromResult(item);
        }

        [UnitOfWork]
        public virtual Task RemoveAsync(string key)
        {
            var cache = GetUnitOfWorkCache();
            if (cache.TryGetValue(key, out _))
            {
                cache[key].IsRemoved = true;
                cache[key].Value = null;
            }
            return Task.CompletedTask;
        }

        protected virtual Dictionary<string, UnitOfWorkCacheItem<TCacheItem>> GetUnitOfWorkCache()
        {
            if (UnitOfWorkManager.Current == null)
            {
                throw new AbpException($"There is no unit of work in the current context, The {GetType().Name} can only be used in a unit of work.");
            }

            return UnitOfWorkManager.Current.Items
                .GetOrAdd(Name, () => new Dictionary<string, UnitOfWorkCacheItem<TCacheItem>>())
                .As<Dictionary<string, UnitOfWorkCacheItem<TCacheItem>>>();
        }
    }

    public abstract class UnitOfWorkCacheWithFallback<TCacheItem> : UnitOfWorkCache<TCacheItem>, IUnitOfWorkCacheWithFallback<TCacheItem>
        where TCacheItem : class
    {
        protected UnitOfWorkCacheWithFallback(IUnitOfWorkManager unitOfWorkManager)
            : base(unitOfWorkManager)
        {
        }

        [UnitOfWork]
        public override async Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory)
        {
            var cache = GetUnitOfWorkCache();
            var value = cache.GetOrDefault(key);
            if (value != null && !value.IsRemoved)
            {
                return value.Value;
            }

            if (value == null)
            {
                var fallbackValue = await GetFallbackCacheItem(key);
                if (fallbackValue != null)
                {
                    return fallbackValue;
                }
            }

            var item = new UnitOfWorkCacheItem<TCacheItem>(await factory());

            if (value == null)
            {
                cache.Add(key, item);
            }
            else
            {
                cache[key] = item;
                cache[key].IsRemoved = false;
            }

            await SetFallbackCacheItem(key, item.Value);

            return item.Value;
        }

        [UnitOfWork]
        public override async Task<TCacheItem> GetOrNullAsync(string key)
        {
            var value = GetUnitOfWorkCache().GetOrDefault(key);
            if (value != null && !value.IsRemoved)
            {
                return value.Value;
            }

            return value != null && value.IsRemoved ? null : await GetFallbackCacheItem(key);
        }

        public abstract Task<TCacheItem> GetFallbackCacheItem(string key);

        public abstract Task<TCacheItem> SetFallbackCacheItem(string key, TCacheItem item);
    }
}
