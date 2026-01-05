using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.StaticDefinitions;

public class StaticDefinitionCache<TKey, TValue> : IStaticDefinitionCache<TKey, TValue>
{
    private Lazy<Task<TValue>>? _lazy;

    public virtual async Task<TValue> GetOrCreateAsync(Func<Task<TValue>> factory)
    {
        var lazy = _lazy;
        if (lazy != null)
        {
            return await lazy.Value;
        }

        var newLazy = new Lazy<Task<TValue>>(factory, LazyThreadSafetyMode.ExecutionAndPublication);
        lazy = Interlocked.CompareExchange(ref _lazy, newLazy, null) ?? newLazy;

        return await lazy.Value;
    }

    public virtual Task ClearAsync()
    {
        Interlocked.Exchange(ref _lazy, null);
        return Task.CompletedTask;
    }
}
