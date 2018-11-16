using System;
using System.Reflection;

namespace Volo.Abp.ObjectMapping
{
    public static class ObjectMapperExtensions
    {
        private static readonly MethodInfo MapToNewObjectMethod;
        private static readonly MethodInfo MapToExistingObjectMethod;

        static ObjectMapperExtensions()
        {
            var methods = typeof(IObjectMapper).GetMethods();
            foreach (var method in methods)
            {
                if (method.Name == nameof(IObjectMapper.Map) && method.IsGenericMethodDefinition)
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1)
                    {
                        MapToNewObjectMethod = method;
                    }
                    else if (parameters.Length == 2)
                    {
                        MapToExistingObjectMethod = method;
                    }
                }
            }
        }

        public static object Map(this IObjectMapper objectMapper, Type sourceType, Type destinationType, object source)
        {
            return MapToNewObjectMethod
                .MakeGenericMethod(sourceType, destinationType)
                .Invoke(objectMapper, new[] { source });
        }

        public static object Map(this IObjectMapper objectMapper, Type sourceType, Type destinationType, object source, object destination)
        {
            return MapToExistingObjectMethod
                .MakeGenericMethod(sourceType, destinationType)
                .Invoke(objectMapper, new[] { source, destination });
        }
    }
}
