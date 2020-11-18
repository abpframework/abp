﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MemoryDb.JsonConverters
{
    public class EntityJsonConverter<TEntity, TKey> : JsonConverter<TEntity>
        where TEntity : Entity<TKey>
    {
        public override TEntity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object)
            {
                var entity = (TEntity)JsonSerializer.Deserialize(jsonDocument.RootElement.GetRawText(), typeToConvert);

                var idJsonElement = jsonDocument.RootElement.GetProperty(nameof(Entity<object>.Id));
                if (idJsonElement.ValueKind != JsonValueKind.Undefined)
                {
                    var id = JsonSerializer.Deserialize<TKey>(idJsonElement.GetRawText());
                    if (id != null)
                    {
                        EntityHelper.TrySetId(entity, () => id);
                    }
                }
                return entity;
            }

            throw new JsonException("RootElement's ValueKind is not Object!");
        }

        public override void Write(Utf8JsonWriter writer, TEntity value, JsonSerializerOptions options)
        {
            var newOptions = JsonSerializerOptionsHelper.Create(options, this);
            var entityConverter = (JsonConverter<TEntity>)newOptions.GetConverter(typeof(TEntity));
            entityConverter.Write(writer, value, newOptions);
        }
    }
}
