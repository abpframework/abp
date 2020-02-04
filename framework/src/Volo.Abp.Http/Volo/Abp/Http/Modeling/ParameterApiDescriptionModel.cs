using System;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ParameterApiDescriptionModel
    {
        public string NameOnMethod { get; set; }

        public string Name { get; set; }

        public string TypeAsString { get; set; }

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
                TypeAsString = type?.GetFullNameWithAssemblyName(),
                IsOptional = isOptional,
                DefaultValue = defaultValue,
                ConstraintTypes = constraintTypes,
                BindingSourceId = bindingSourceId,
                DescriptorName = descriptorName
            };
        }
    }
}