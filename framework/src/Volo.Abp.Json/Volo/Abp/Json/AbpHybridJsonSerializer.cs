using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json
{
    public class AbpHybridJsonSerializer : IJsonSerializer, ITransientDependency
    {
        protected AbpJsonOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }

        public AbpHybridJsonSerializer(IOptions<AbpJsonOptions> options, IServiceProvider serviceProvider)
        {
            Options = options.Value;
            ServiceProvider = serviceProvider;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            var provider = GetSerializerProvider(obj.GetType());
            return provider.Serialize(obj, camelCase, indented);
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            var provider = GetSerializerProvider(typeof(T));
            return provider.Deserialize<T>(jsonString, camelCase);
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            var provider = GetSerializerProvider(type);
            return provider.Deserialize(type, jsonString, camelCase);
        }

        protected virtual IJsonSerializerProvider GetSerializerProvider(Type type)
        {
            foreach (var providerType in Options.Providers.Reverse())
            {
                var provider = ServiceProvider.GetRequiredService(providerType) as IJsonSerializerProvider;
                if (provider.CanHandle(type))
                {
                    return provider;
                }
            }

            throw new AbpException($"There is no IJsonSerializerProvider that can handle '{type.GetFullNameWithAssemblyName()}'!");
        }
    }
}
