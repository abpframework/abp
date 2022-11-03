using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;
using Volo.Abp.Dapr;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Json;

public class AbpDaprSubscriptionRequestConverter<T> : JsonConverter<AbpDaprSubscriptionRequest<T>>
    where T : class
{
    private JsonSerializerOptions _readJsonSerializerOptions;

    private readonly IDaprSerializer _daprSerializer;

    public AbpDaprSubscriptionRequestConverter(IDaprSerializer daprSerializer)
    {
        _daprSerializer = daprSerializer;
    }

    public override AbpDaprSubscriptionRequest<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        _readJsonSerializerOptions ??= CreateJsonSerializerOptions(options);

        var rootElement = JsonDocument.ParseValue(ref reader).RootElement;
        var obj = JsonSerializer.Deserialize<AbpDaprSubscriptionRequest<T>>(rootElement.GetRawText(), _readJsonSerializerOptions);
        obj.Data = _daprSerializer.Deserialize(rootElement.GetProperty("data").GetRawText(), typeof(T)).As<T>();
        return obj;
    }

    public override void Write(Utf8JsonWriter writer, AbpDaprSubscriptionRequest<T> value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    private JsonSerializerOptions CreateJsonSerializerOptions(JsonSerializerOptions options)
    {
        var newOptions = new JsonSerializerOptions(options);
        newOptions.Converters.RemoveAll(x => x == this || x.GetType() == typeof(AbpDaprSubscriptionRequestConverterFactory));
        newOptions.PropertyNamingPolicy = new AbpDaprSubscriptionRequestJsonNamingPolicy();
        return newOptions;
    }
}
