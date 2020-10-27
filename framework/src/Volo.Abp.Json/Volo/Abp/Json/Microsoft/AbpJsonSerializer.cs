using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.Microsoft
{
    public class AbpJsonSerializer : IJsonSerializer, ITransientDependency
    {
        protected AbpJsonSerializerOptions Options { get; }

        public AbpJsonSerializer(IOptions<AbpJsonSerializerOptions> options)
        {
            Options = options.Value;
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
