using System;
using System.Reflection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class PropertyApiDescriptionModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string TypeSimple { get; set; }

        //TODO: Validation rules for this property
        public static PropertyApiDescriptionModel Create(PropertyInfo propertyInfo)
        {
            string typeName;
            string simpleTypeName;

            if (TypeHelper.IsEnumerable(propertyInfo.PropertyType, out var itemType, includePrimitives: false))
            {
                typeName = $"[{TypeHelper.GetFullNameHandlingNullableAndGenerics(itemType)}]";
                simpleTypeName = $"[{TypeHelper.GetSimplifiedName(itemType)}]";
            }
            else if (TypeHelper.IsDictionary(propertyInfo.PropertyType, out var keyType, out var valueType))
            {
                typeName = $"{{{TypeHelper.GetFullNameHandlingNullableAndGenerics(keyType)}:{TypeHelper.GetFullNameHandlingNullableAndGenerics(valueType)}}}";
                simpleTypeName = $"{{{TypeHelper.GetSimplifiedName(keyType)}:{TypeHelper.GetSimplifiedName(valueType)}}}";
            }
            else
            {
                typeName = TypeHelper.GetFullNameHandlingNullableAndGenerics(propertyInfo.PropertyType);
                simpleTypeName = TypeHelper.GetSimplifiedName(propertyInfo.PropertyType);
            }
            
            return new PropertyApiDescriptionModel
            {
                Name = propertyInfo.Name,
                Type = typeName,
                TypeSimple = simpleTypeName
            };
        }
    }
}