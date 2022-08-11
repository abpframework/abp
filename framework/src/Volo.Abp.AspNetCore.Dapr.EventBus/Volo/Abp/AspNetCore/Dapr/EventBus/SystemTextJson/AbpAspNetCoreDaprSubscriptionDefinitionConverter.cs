using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.AspNetCore.Dapr.Models;

namespace Volo.Abp.AspNetCore.Dapr.SystemTextJson;

public class AbpAspNetCoreDaprSubscriptionDefinitionConverter : JsonConverter<AbpAspNetCoreDaprSubscriptionDefinition>
{
    private JsonSerializerOptions _writeJsonSerializerOptions;

    public override AbpAspNetCoreDaprSubscriptionDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, AbpAspNetCoreDaprSubscriptionDefinition value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(new JsonSerializerOptions(options)
        {
            PropertyNamingPolicy = new AbpAspNetCoreDaprPubSubJsonNamingPolicy()
        }, x => x == this);

        JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
    }
}
