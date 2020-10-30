using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.Json.SystemTextJson
{
    public class SystemTextJsonSupportTypes : ITransientDependency
    {
        private static readonly List<Type> CacheTypes = new List<Type>();

        private readonly SystemTextJsonSupportTypesOptions Options;

        public SystemTextJsonSupportTypes(IOptions<SystemTextJsonSupportTypesOptions> options)
        {
            Options = options.Value;
        }

        public bool CanHandle(Type type)
        {
            if (CacheTypes.Any(x => x == type))
            {
                return false;
            }

            if (type.GetCustomAttributes(true).Any(x => Options.IgnoreAttributes.Any(a => a == x.GetType())))
            {
                CacheTypes.Add(type);
                return false;
            }

            if (type.DeclaringType != null && type.DeclaringType.GetCustomAttributes(true).Any(x => Options.IgnoreAttributes.Any(a => a == x.GetType())))
            {
                CacheTypes.Add(type.DeclaringType);
                return false;
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true))
                {
                    CacheTypes.Add(propertyInfo.GetType());
                    return false;
                }

                if (!TypeHelper.IsPrimitiveExtended(type))
                {
                    if (!CanHandle(propertyInfo.PropertyType))
                    {
                        CacheTypes.Add(propertyInfo.PropertyType);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
