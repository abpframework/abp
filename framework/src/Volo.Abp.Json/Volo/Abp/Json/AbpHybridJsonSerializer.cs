using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json
{
    public class AbpHybridJsonSerializer : IJsonSerializer, ISingletonDependency
    {
        private readonly Lazy<List<IJsonSerializerProvider>> _lazyProviders;
        public List<IJsonSerializerProvider> Providers => _lazyProviders.Value;

        public AbpHybridJsonSerializer(IOptions<AbpJsonOptions> options, IServiceProvider serviceProvider)
        {
            _lazyProviders = new Lazy<List<IJsonSerializerProvider>>(
                () => options.Value
                    .Providers
                    .Select(c => serviceProvider.GetRequiredService(c) as IJsonSerializerProvider)
                    .Reverse()
                    .ToList(),
                true
            );
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
            foreach (var provider in Providers.Where(provider => provider.CanHandle(type)))
            {
                return provider;
            }

            throw new AbpException($"There is no IJsonSerializerProvider that can handle {type.GetFullNameWithAssemblyName()} types!");
        }
    }
}
