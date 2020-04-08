using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Modeling
{
    public static class ModelingTypeHelper
    {
        public static string GetFullNameHandlingNullableAndGenerics([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GenericTypeArguments[0].FullName + "?";
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                return $"{genericType.FullName.Left(genericType.FullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetFullNameHandlingNullableAndGenerics).JoinAsString(",")}>";
            }

            return type.FullName;
        }

        public static string GetSimplifiedName([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetSimplifiedName(type.GenericTypeArguments[0]) + "?";
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                return $"{genericType.FullName.Left(genericType.FullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetSimplifiedName).JoinAsString(",")}>";
            }

            if (type == typeof(string))
            {
                return "string";
            }
            else if (type == typeof(int))
            {
                return "number";
            }
            else if (type == typeof(long))
            {
                return "number";
            }
            else if (type == typeof(bool))
            {
                return "boolean";
            }
            else if (type == typeof(char))
            {
                return "string";
            }
            else if (type == typeof(double))
            {
                return "number";
            }
            else if (type == typeof(float))
            {
                return "number";
            }
            else if (type == typeof(decimal))
            {
                return "number";
            }
            else if (type == typeof(DateTime))
            {
                return "string";
            }
            else if (type == typeof(DateTimeOffset))
            {
                return "string";
            }
            else if (type == typeof(TimeSpan))
            {
                return "string";
            }
            else if (type == typeof(Guid))
            {
                return "string";
            }
            else if (type == typeof(byte))
            {
                return "number";
            }
            else if (type == typeof(sbyte))
            {
                return "number";
            }
            else if (type == typeof(short))
            {
                return "number";
            }
            else if (type == typeof(ushort))
            {
                return "number";
            }
            else if (type == typeof(uint))
            {
                return "number";
            }
            else if (type == typeof(ulong))
            {
                return "number";
            }
            else if (type == typeof(IntPtr))
            {
                return "number";
            }
            else if (type == typeof(UIntPtr))
            {
                return "number";
            }

            return type.FullName;
        }
    }
}
