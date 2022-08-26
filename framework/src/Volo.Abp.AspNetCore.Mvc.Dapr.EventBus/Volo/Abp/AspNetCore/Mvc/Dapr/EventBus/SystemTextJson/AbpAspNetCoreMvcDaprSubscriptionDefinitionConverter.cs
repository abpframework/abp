using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.SystemTextJson;

public class AbpAspNetCoreMvcDaprSubscriptionDefinitionConverter : JsonConverter<AbpAspNetCoreMvcDaprSubscriptionDefinition>
{
    private JsonSerializerOptions _writeJsonSerializerOptions;

    public override AbpAspNetCoreMvcDaprSubscriptionDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, AbpAspNetCoreMvcDaprSubscriptionDefinition value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(new JsonSerializerOptions(options)
        {
            PropertyNamingPolicy = new AbpAspNetCoreMvcDaprPubSubJsonNamingPolicy()
        }, x => x == this);

        JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
    }
}
