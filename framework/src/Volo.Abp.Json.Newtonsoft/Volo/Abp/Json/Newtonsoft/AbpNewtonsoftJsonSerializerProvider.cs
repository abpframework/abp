using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
namespace Volo.Abp.Json.Newtonsoft;

public class AbpNewtonsoftJsonSerializerProvider : IJsonSerializerProvider, ITransientDependency
{
    protected IServiceProvider ServiceProvider{ get; }
    protected List<JsonConverter> Converters { get; }

    public AbpNewtonsoftJsonSerializerProvider(
        IServiceProvider serviceProvider,
        IOptions<AbpNewtonsoftJsonSerializerOptions> options)
    {
        ServiceProvider = serviceProvider;
        Converters = options.Value
            .Converters
            .Select(c => (JsonConverter)serviceProvider.GetRequiredService(c))
            .ToList();
    }

    public bool CanHandle(Type type)
    {
        return true;
    }

    public string Serialize(object obj, bool camelCase = true, bool indented = false)
    {
        return JsonConvert.SerializeObject(obj, CreateJsonSerializerOptions(camelCase, indented));
    }

    public T Deserialize<T>(string jsonString, bool camelCase = true)
    {
        return JsonConvert.DeserializeObject<T>(jsonString, CreateJsonSerializerOptions(camelCase));
    }

    public object Deserialize(Type type, string jsonString, bool camelCase = true)
    {
        return JsonConvert.DeserializeObject(jsonString, type, CreateJsonSerializerOptions(camelCase));
    }

    private readonly static ConcurrentDictionary<object, JsonSerializerSettings> JsonSerializerOptionsCache = new ConcurrentDictionary<object, JsonSerializerSettings>();

    protected virtual JsonSerializerSettings CreateJsonSerializerOptions(bool camelCase = true, bool indented = false)
    {
        return JsonSerializerOptionsCache.GetOrAdd(new
        {
            camelCase,
            indented
        }, _ =>
        {
            var settings = new JsonSerializerSettings();

            settings.Converters.InsertRange(0, Converters);

            if (camelCase)
            {
                settings.ContractResolver = ServiceProvider.GetRequiredService<AbpCamelCasePropertyNamesContractResolver>();
            }

            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }

            return settings;
        });
    }
}
