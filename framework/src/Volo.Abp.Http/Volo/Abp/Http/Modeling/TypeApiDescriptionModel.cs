using System;
using System.Linq;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class TypeApiDescriptionModel
{
    public string BaseType { get; set; }

    public bool IsEnum { get; set; }

    public string[] EnumNames { get; set; }

    public object[] EnumValues { get; set; }

    public string[] GenericArguments { get; set; }

    public PropertyApiDescriptionModel[] Properties { get; set; }

    public TypeApiDescriptionModel()
    {

    }

    public static TypeApiDescriptionModel Create(Type type)
    {
        var baseType = type.BaseType;
        if (baseType == typeof(object))
        {
            baseType = null;
        }

        var typeModel = new TypeApiDescriptionModel
        {
            IsEnum = type.IsEnum,
            BaseType = baseType != null ? TypeHelper.GetFullNameHandlingNullableAndGenerics(baseType) : null
        };

        if (typeModel.IsEnum)
        {
            typeModel.EnumNames = type.GetEnumNames();
            typeModel.EnumValues = type.GetEnumValues().Cast<object>().ToArray();
        }
        else
        {
            typeModel.Properties = type
                .GetProperties()
                .Where(p => p.DeclaringType == type)
                .Select(PropertyApiDescriptionModel.Create)
                .ToArray();

            if (type.IsGenericTypeDefinition)
            {
                typeModel.GenericArguments = type.GetGenericArguments().Select(a => a.Name).ToArray();
            }
        }

        return typeModel;
    }
}
