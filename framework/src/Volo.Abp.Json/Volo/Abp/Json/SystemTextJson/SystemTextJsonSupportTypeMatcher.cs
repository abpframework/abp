using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson
{
    public class SystemTextJsonSupportTypeMatcher : ITransientDependency
    {
        private static readonly ConcurrentBag<Type> CacheTypes = new ConcurrentBag<Type>();

        private readonly SystemTextJsonSupportTypeMatcherOptions _options;

        public SystemTextJsonSupportTypeMatcher(IOptions<SystemTextJsonSupportTypeMatcherOptions> options)
        {
            _options = options.Value;
        }

        public bool Match(Type type)
        {
            if (_options.UnsupportedTypes.Any(x => x == type))
            {
                return false;
            }

            if (CacheTypes.Contains(type))
            {
                return false;
            }

            if (type.GetCustomAttributes(true).Any(x => _options.UnsupportedAttributes.Any(a => a == x.GetType())))
            {
                CacheTypes.Add(type);
                return false;
            }

            if (type.DeclaringType != null && type.DeclaringType.GetCustomAttributes(true).Any(x => _options.UnsupportedAttributes.Any(a => a == x.GetType())))
            {
                CacheTypes.Add(type);
                return false;
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true))
                {
                    CacheTypes.Add(type);
                    return false;
                }

                if (!TypeHelper.IsPrimitiveExtended(type, includeNullables: true, includeEnums: true))
                {
                    if (!Match(propertyInfo.PropertyType))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
