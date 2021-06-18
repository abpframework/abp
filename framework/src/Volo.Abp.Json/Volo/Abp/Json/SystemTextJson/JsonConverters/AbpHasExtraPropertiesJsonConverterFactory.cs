﻿using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters
{
    public class AbpHasExtraPropertiesJsonConverterFactory : JsonConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, bool> CachedTypes = new ConcurrentDictionary<Type, bool>();

        public override bool CanConvert(Type typeToConvert)
        {
            //Only for private or protected ExtraProperties.
            if (typeof(IHasExtraProperties).IsAssignableFrom(typeToConvert))
            {
                return CachedTypes.GetOrAdd(typeToConvert, type =>
                {
                    var property = type.GetProperty(nameof(IHasExtraProperties.ExtraProperties));
                    if (property != null)
                    {
                        var setMethod = property.GetSetMethod(true);
                        return setMethod != null && (setMethod.IsPrivate || setMethod.IsFamily);
                    }

                    return false;
                });
            }

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter) Activator.CreateInstance(
                typeof(AbpHasExtraPropertiesJsonConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                null,
                culture: null)!;
        }
    }
}
