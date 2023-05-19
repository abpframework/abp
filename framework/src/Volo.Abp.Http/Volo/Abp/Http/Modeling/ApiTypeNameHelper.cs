using System;
using Volo.Abp.Collections;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling;

public static class ApiTypeNameHelper
{
    public static string GetTypeName(Type type)
    {
        return GetTypeName(type, new TypeList());
    }

    private static string GetTypeName(Type type, ITypeList duplicateTypes)
    {
        duplicateTypes.Add(type);

        if (TypeHelper.IsDictionary(type, out var keyType, out var valueType))
        {
            if (!duplicateTypes.Contains(keyType) && !duplicateTypes.Contains(valueType))
            {
                return $"{{{GetTypeName(keyType, duplicateTypes)}:{GetTypeName(valueType, duplicateTypes)}}}";
            }
        }
        else if (TypeHelper.IsEnumerable(type, out var itemType, includePrimitives: false))
        {
            if (!duplicateTypes.Contains(itemType))
            {
                return $"[{GetTypeName(itemType, duplicateTypes)}]";
            }
        }

        return TypeHelper.GetFullNameHandlingNullableAndGenerics(type);
    }

    public static string GetSimpleTypeName(Type type)
    {
        return GetSimpleTypeName(type, new TypeList());
    }

    private static string GetSimpleTypeName(Type type, ITypeList duplicateTypes)
    {
        duplicateTypes.Add(type);

        if (TypeHelper.IsDictionary(type, out var keyType, out var valueType))
        {
            if (!duplicateTypes.Contains(keyType) && !duplicateTypes.Contains(valueType))
            {
                return $"{{{GetSimpleTypeName(keyType, duplicateTypes)}:{GetSimpleTypeName(valueType, duplicateTypes)}}}";
            }
        }
        else if (TypeHelper.IsEnumerable(type, out var itemType, includePrimitives: false))
        {
            if (!duplicateTypes.Contains(itemType))
            {
                return $"[{GetSimpleTypeName(itemType, duplicateTypes)}]";
            }
        }

        return TypeHelper.GetSimplifiedName(type);
    }
}
