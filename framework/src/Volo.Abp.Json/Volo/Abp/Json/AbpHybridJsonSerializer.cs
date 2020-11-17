﻿using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json
{
    public class AbpHybridJsonSerializer : IJsonSerializer, ITransientDependency
    {
        protected AbpJsonOptions Options { get; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public AbpHybridJsonSerializer(IOptions<AbpJsonOptions> options, IServiceScopeFactory serviceScopeFactory)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var serializerProvider = GetSerializerProvider(scope.ServiceProvider, obj.GetType());
                return serializerProvider.Serialize(obj, camelCase, indented);
            }
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var serializerProvider = GetSerializerProvider(scope.ServiceProvider, typeof(T));
                return serializerProvider.Deserialize<T>(jsonString, camelCase);
            }
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var serializerProvider = GetSerializerProvider(scope.ServiceProvider, type);
                return serializerProvider.Deserialize(type, jsonString, camelCase);
            }
        }

        protected virtual IJsonSerializerProvider GetSerializerProvider(IServiceProvider serviceProvider, Type type)
        {
            foreach (var providerType in Options.Providers.Reverse())
            {
                var provider = serviceProvider.GetRequiredService(providerType) as IJsonSerializerProvider;
                if (provider.CanHandle(type))
                {
                    return provider;
                }
            }

            throw new AbpException($"There is no IJsonSerializerProvider that can handle '{type.GetFullNameWithAssemblyName()}'!");
        }
    }
}
