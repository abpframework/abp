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
        private static readonly ConcurrentBag<Type> SupportedTypesCache = new ConcurrentBag<Type>();
        private static readonly ConcurrentBag<Type> UnsupportedTypesCache = new ConcurrentBag<Type>();

        private readonly SystemTextJsonSupportTypeMatcherOptions _options;

        public SystemTextJsonSupportTypeMatcher(IOptions<SystemTextJsonSupportTypeMatcherOptions> options)
        {
            _options = options.Value;
        }

        public bool Match(Type type)
        {
            if (UnsupportedTypesCache.Contains(type))
            {
                return false;
            }

            if (SupportedTypesCache.Contains(type))
            {
                return true;
            }

            if (_options.UnsupportedTypes.Any(x => x == type))
            {
                UnsupportedTypesCache.Add(type);
                return false;
            }

            if (type.IsGenericType)
            {
                foreach (var genericArgument in type.GetGenericArguments())
                {
                    if (!TypeHelper.IsPrimitiveExtended(genericArgument, includeNullables: true, includeEnums: true))
                    {
                        if (!Match(genericArgument))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (_options.UnsupportedTypes.Any(x => x == genericArgument))
                        {
                            UnsupportedTypesCache.Add(genericArgument);
                            return false;
                        }
                    }
                }

                return true;
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                if (!TypeHelper.IsPrimitiveExtended(elementType, includeNullables: true, includeEnums: true))
                {
                    if (!Match(elementType))
                    {
                        return false;
                    }
                }
                else
                {
                    if (_options.UnsupportedTypes.Any(x => x == elementType))
                    {
                        UnsupportedTypesCache.Add(elementType);
                        return false;
                    }
                }

                return true;
            }


            if (TypeHelper.IsPrimitiveExtended(type, includeNullables: true, includeEnums: true))
            {
                return true;
            }

            if (type.GetCustomAttributes(true).Any(x => _options.UnsupportedAttributes.Any(a => a == x.GetType())))
            {
                UnsupportedTypesCache.Add(type);
                return false;
            }

            if (type.DeclaringType != null && type.DeclaringType.GetCustomAttributes(true).Any(x => _options.UnsupportedAttributes.Any(a => a == x.GetType())))
            {
                UnsupportedTypesCache.Add(type);
                return false;
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true))
                {
                    UnsupportedTypesCache.Add(type);
                    return false;
                }

                if (_options.UnsupportedTypes.Any(x => x == propertyInfo.PropertyType))
                {
                    UnsupportedTypesCache.Add(propertyInfo.PropertyType);
                    return false;
                }

                if (propertyInfo.PropertyType.IsGenericType)
                {
                    foreach (var genericArgument in propertyInfo.PropertyType.GetGenericArguments())
                    {
                        if (!TypeHelper.IsPrimitiveExtended(genericArgument, includeNullables: true, includeEnums: true))
                        {
                            if (!Match(genericArgument))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (_options.UnsupportedTypes.Any(x => x == genericArgument))
                            {
                                UnsupportedTypesCache.Add(genericArgument);
                                return false;
                            }
                        }
                    }
                }

                if (propertyInfo.PropertyType.IsArray)
                {
                    var elementType = propertyInfo.PropertyType.GetElementType();
                    if (!TypeHelper.IsPrimitiveExtended(elementType, includeNullables: true, includeEnums: true))
                    {
                        if (!Match(elementType))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (_options.UnsupportedTypes.Any(x => x == elementType))
                        {
                            UnsupportedTypesCache.Add(elementType);
                            return false;
                        }
                    }
                }

                if (!TypeHelper.IsPrimitiveExtended(propertyInfo.PropertyType, includeNullables: true, includeEnums: true))
                {
                    if (!Match(propertyInfo.PropertyType))
                    {
                        return false;
                    }
                }
                else
                {
                    if (_options.UnsupportedTypes.Any(x => x == propertyInfo.PropertyType))
                    {
                        UnsupportedTypesCache.Add(propertyInfo.PropertyType);
                        return false;
                    }
                }
            }

            SupportedTypesCache.Add(type);
            return true;
        }
    }
}
