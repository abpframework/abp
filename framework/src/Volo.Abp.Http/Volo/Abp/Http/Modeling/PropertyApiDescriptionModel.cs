using System;
using System.Reflection;

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
            return new PropertyApiDescriptionModel
            {
                Name = propertyInfo.Name,
                Type = ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(propertyInfo.PropertyType),
                TypeSimple = ModelingTypeHelper.GetSimplifiedName(propertyInfo.PropertyType)
            };
        }
    }
}