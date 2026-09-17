using System;
using System.Threading.Tasks;

namespace Volo.Abp.StaticDefinitions;

public interface IStaticDefinitionCache<TKey, TValue>
{
    Task<TValue> GetOrCreateAsync(Func<Task<TValue>> factory);

    Task ClearAsync();
}
