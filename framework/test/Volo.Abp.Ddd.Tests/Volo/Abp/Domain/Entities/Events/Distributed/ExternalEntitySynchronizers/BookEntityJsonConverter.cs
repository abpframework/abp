using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers;

public class BookEntityJsonConverter : JsonConverter<Book>
{
    private JsonSerializerOptions _writeJsonSerializerOptions;

    public override Book Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);

        if (jsonDocument.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("RootElement's ValueKind is not Object!");
        }

        var entity = (Book)jsonDocument.RootElement.Deserialize(typeToConvert);

        if (entity == null)
        {
            throw new JsonException("RootElement's ValueKind is not Object!");
        }

        ObjectHelper.TrySetProperty(entity, x => x.RemoteLastModificationTime, () =>
        {
            var property = jsonDocument.RootElement.GetProperty("RemoteLastModificationTime");

            return property.ValueKind == JsonValueKind.Null ? null : property.GetDateTime();
        });

        return entity;
    }

    public override void Write(Utf8JsonWriter writer, Book value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
        JsonSerializer.Serialize(writer, value, _writeJsonSerializerOptions);
    }
}