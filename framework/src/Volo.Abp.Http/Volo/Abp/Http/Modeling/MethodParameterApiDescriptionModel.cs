using System;
using System.Reflection;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class MethodParameterApiDescriptionModel
    {
        public string Name { get; set; }

        public string TypeAsString { get; set; }

        public string Type { get; set; }

        public string TypeSimple { get; set; }

        public bool IsOptional { get; set; }

        public object DefaultValue { get; set; }

        private MethodParameterApiDescriptionModel()
        {

        }

        public static MethodParameterApiDescriptionModel Create(ParameterInfo parameterInfo)
        {
            return new MethodParameterApiDescriptionModel
            {
                Name = parameterInfo.Name,
                TypeAsString = parameterInfo.ParameterType.GetFullNameWithAssemblyName(),
                Type = parameterInfo.ParameterType != null ? ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(parameterInfo.ParameterType) : null,
                TypeSimple = parameterInfo.ParameterType != null ? ModelingTypeHelper.GetSimplifiedName(parameterInfo.ParameterType) : null,
                IsOptional = parameterInfo.IsOptional,
                DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null
            };
        }
    }
}