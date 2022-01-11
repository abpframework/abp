using System;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.SystemTextJson
{
    public class AbpSystemTextJsonSerializerProvider : IJsonSerializerProvider, ITransientDependency
    {
        protected AbpSystemTextJsonSerializerOptions Options { get; }

        protected AbpSystemTextJsonUnsupportedTypeMatcher AbpSystemTextJsonUnsupportedTypeMatcher { get; }

        public AbpSystemTextJsonSerializerProvider(
            IOptions<AbpSystemTextJsonSerializerOptions> options,
            AbpSystemTextJsonUnsupportedTypeMatcher abpSystemTextJsonUnsupportedTypeMatcher)
        {
            AbpSystemTextJsonUnsupportedTypeMatcher = abpSystemTextJsonUnsupportedTypeMatcher;
            Options = options.Value;
        }

        public bool CanHandle(Type type)
        {
            return !AbpSystemTextJsonUnsupportedTypeMatcher.Match(type);
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

        private readonly ConcurrentDictionary<string, JsonSerializerOptions> JsonSerializerOptionsCache = new ConcurrentDictionary<string, JsonSerializerOptions>();

        protected virtual JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = true, bool indented = false)
        {
            return JsonSerializerOptionsCache.GetOrAdd($"default{camelCase}{indented}", _ =>
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
            });
        }
    }
}
