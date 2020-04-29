using System;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ParameterApiDescriptionModel
    {
        public string NameOnMethod { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string TypeSimple { get; set; }

        public bool IsOptional { get; set; }

        public object DefaultValue { get; set; }

        public string[] ConstraintTypes { get; set; }

        public string BindingSourceId { get; set; }

        public string DescriptorName { get; set; }

        private ParameterApiDescriptionModel()
        {
            
        }

        public static ParameterApiDescriptionModel Create(string name, string nameOnMethod, Type type, bool isOptional = false, object defaultValue = null, string[] constraintTypes = null, string bindingSourceId = null, string descriptorName = null)
        {
            return new ParameterApiDescriptionModel
            {
                Name = name,
                NameOnMethod = nameOnMethod,
                Type = type != null ? TypeHelper.GetFullNameHandlingNullableAndGenerics(type) : null,
                TypeSimple = type != null ? TypeHelper.GetSimplifiedName(type) : null,
                IsOptional = isOptional,
                DefaultValue = defaultValue,
                ConstraintTypes = constraintTypes,
                BindingSourceId = bindingSourceId,
                DescriptorName = descriptorName
            };
        }
    }
}