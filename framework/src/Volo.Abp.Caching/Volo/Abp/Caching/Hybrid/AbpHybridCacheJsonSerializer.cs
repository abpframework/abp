using System.Buffers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Hybrid;

namespace Volo.Abp.Caching.Hybrid;

public class AbpHybridCacheJsonSerializer<T> : IHybridCacheSerializer<T>
{
    protected JsonSerializerOptions JsonSerializerOptions { get; }

    public AbpHybridCacheJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        JsonSerializerOptions = jsonSerializerOptions;
    }

    public virtual T Deserialize(ReadOnlySequence<byte> source)
    {
        var reader = new Utf8JsonReader(source);
        return JsonSerializer.Deserialize<T>(ref reader, JsonSerializerOptions)!;
    }

    public virtual void Serialize(T value, IBufferWriter<byte> target)
    {
        using var writer = new Utf8JsonWriter(target);
        JsonSerializer.Serialize<T>(writer, value, JsonSerializerOptions);
    }
}
