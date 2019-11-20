using System;
using System.Reflection;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class MethodParameterApiDescriptionModel
    {
        public string Name { get; set; }

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
                Type = ModelingTypeHelper.GetFullNameHandlingNullableAndGenerics(parameterInfo.ParameterType),
                TypeSimple = ModelingTypeHelper.GetSimplifiedName(parameterInfo.ParameterType),
                IsOptional = parameterInfo.IsOptional,
                DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null
            };
        }
    }
}