using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.AspNetCore.Dapr.Models;

namespace Volo.Abp.AspNetCore.Dapr.SystemTextJson;

public class DaprSubscriptionDefinitionConverter : JsonConverter<DaprSubscriptionDefinition>
{
    private JsonSerializerOptions _writeJsonSerializerOptions;

    public override DaprSubscriptionDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, DaprSubscriptionDefinition value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(new JsonSerializerOptions(options)
        {
            PropertyNamingPolicy = new AbpAspNetCoreDaprJsonNamingPolicy()
        }, x => x == this);

        JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
    }
}
