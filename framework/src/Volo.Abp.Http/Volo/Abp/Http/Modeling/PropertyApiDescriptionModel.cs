using System;
using System.Reflection;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class PropertyApiDescriptionModel
    {
        public string Name { get; set; }

        public string TypeAsString { get; set; }

        //TODO: Validation rules for this property
        public static PropertyApiDescriptionModel Create(PropertyInfo propertyInfo)
        {
            return new PropertyApiDescriptionModel
            {
                Name = propertyInfo.Name,
                TypeAsString = propertyInfo.PropertyType.FullName
            };
        }
    }
}