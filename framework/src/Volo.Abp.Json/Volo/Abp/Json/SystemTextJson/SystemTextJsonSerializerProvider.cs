using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace Volo.Abp.Json
{
    public class SystemTextJsonSerializerProvider : IJsonSerializerProvider, ITransientDependency
    {
        protected AbpSystemTextJsonSerializerOptions Options { get; }

        protected SystemTextJsonSupportTypes SystemTextJsonSupportTypes { get; }

        public SystemTextJsonSerializerProvider(IOptions<AbpSystemTextJsonSerializerOptions> options, SystemTextJsonSupportTypes systemTextJsonSupportTypes)
        {
            SystemTextJsonSupportTypes = systemTextJsonSupportTypes;
            Options = options.Value;
        }

        public bool CanHandle(Type type)
        {
            return SystemTextJsonSupportTypes.CanHandle(type);
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            return JsonSerializer.Serialize(obj, CreateJsonSerializerOptions(camelCase, indented));
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            return JsonSerializer.Deserialize<T>(jsonString, CreateJsonSerializerOptions(camelCase));
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            return JsonSerializer.Deserialize(jsonString, type, CreateJsonSerializerOptions(camelCase));
        }

        protected virtual JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = true, bool indented = false)
        {
            var settings = new JsonSerializerOptions(Options.JsonSerializerOptions);

            if (camelCase)
            {
                settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            }

            if (indented)
            {
                settings.WriteIndented = true;
            }

            return settings;
        }
    }
}
