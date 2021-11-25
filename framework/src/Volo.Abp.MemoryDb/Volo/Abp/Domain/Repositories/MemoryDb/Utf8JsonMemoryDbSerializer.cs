using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Domain.Repositories.MemoryDb;

public class Utf8JsonMemoryDbSerializer : IMemoryDbSerializer, ITransientDependency
{
    protected Utf8JsonMemoryDbSerializerOptions Options { get; }

    public Utf8JsonMemoryDbSerializer(IOptions<Utf8JsonMemoryDbSerializerOptions> options)
    {
        Options = options.Value;
    }

    byte[] IMemoryDbSerializer.Serialize(object obj)
    {
        return JsonSerializer.SerializeToUtf8Bytes(obj, Options.JsonSerializerOptions);
    }

    public object Deserialize(byte[] value, Type type)
    {
        return JsonSerializer.Deserialize(value, type, Options.JsonSerializerOptions);
    }
}
