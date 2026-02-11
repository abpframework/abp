using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.Caching.Hybrid;

public class AbpHybridCacheJsonSerializerFactory : IHybridCacheSerializerFactory
{
    protected IOptions<AbpSystemTextJsonSerializerOptions> Options { get; }

    public AbpHybridCacheJsonSerializerFactory(IOptions<AbpSystemTextJsonSerializerOptions> options)
    {
        Options = options;
    }

    public bool TryCreateSerializer<T>(out IHybridCacheSerializer<T>? serializer)
    {
        if (typeof(T) == typeof(string) || typeof(T) == typeof(byte[]))
        {
            serializer = null;
            return false;
        }

        serializer = new AbpHybridCacheJsonSerializer<T>(Options.Value.JsonSerializerOptions);
        return true;
    }
}
