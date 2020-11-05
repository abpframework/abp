using System;
using Volo.Abp.Collections;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling
{
    public static class ApiTypeNameHelper
    {
        private static ITypeList _cycleType = new TypeList();

        public static string GetTypeName(Type type)
        {
            if (TypeHelper.IsDictionary(type, out var keyType, out var valueType))
            {
                if (keyType != type && valueType != type)
                {
                    return $"{{{GetTypeName(keyType)}:{GetTypeName(valueType)}}}";
                }
            }
            else if (TypeHelper.IsEnumerable(type, out var itemType, includePrimitives: false))
            {
                if (itemType != type)
                {
                    return $"[{GetTypeName(itemType)}]";
                }
            }

            return TypeHelper.GetFullNameHandlingNullableAndGenerics(type);
        }

        public static string GetSimpleTypeName(Type type)
        {
            if (TypeHelper.IsDictionary(type, out var keyType, out var valueType))
            {
                if (keyType != type && valueType != type)
                {
                    return  $"{{{GetSimpleTypeName(keyType)}:{GetSimpleTypeName(valueType)}}}";
                }
            }
            else if (TypeHelper.IsEnumerable(type, out var itemType, includePrimitives: false))
            {
                if (itemType != type)
                {
                    return  $"[{GetSimpleTypeName(itemType)}]";
                }
            }

            return TypeHelper.GetSimplifiedName(type);
        }
    }
}
