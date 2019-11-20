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
                typeName = $"[{ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(itemType)}]";
                simpleTypeName = $"[{ModelingTypeHelper.GetSimplifiedName(itemType)}]";
            }
            else if (TypeHelper.IsDictionary(propertyInfo.PropertyType, out var keyType, out var valueType))
            {
                typeName = $"{{{ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(keyType)}:{ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(valueType)}}}";
                simpleTypeName = $"{{{ModelingTypeHelper.GetSimplifiedName(keyType)}:{ModelingTypeHelper.GetSimplifiedName(valueType)}}}";
            }
            else
            {
                typeName = ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(propertyInfo.PropertyType);
                simpleTypeName = ModelingTypeHelper.GetSimplifiedName(propertyInfo.PropertyType);
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