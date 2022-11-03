using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;
using Volo.Abp.Dapr;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Json;

public class AbpDaprSubscriptionRequestConverterFactory : JsonConverterFactory
{
    private readonly IDaprSerializer _daprSerializer;

    public AbpDaprSubscriptionRequestConverterFactory(IDaprSerializer daprSerializer)
    {
        _daprSerializer = daprSerializer;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.GetGenericTypeDefinition() == typeof(AbpDaprSubscriptionRequest<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (JsonConverter)Activator.CreateInstance(
            typeof(AbpDaprSubscriptionRequestConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            new object[] { _daprSerializer },
            culture: null)!;
    }
}
