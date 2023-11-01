using System;
using System.Reflection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class MethodParameterApiDescriptionModel
{
    public string Name { get; set; } = default!;

    public string TypeAsString { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string TypeSimple { get; set; } = default!;

    public bool IsOptional { get; set; }

    public object? DefaultValue { get; set; }

    public MethodParameterApiDescriptionModel()
    {

    }

    public static MethodParameterApiDescriptionModel Create(ParameterInfo parameterInfo)
    {
        return new MethodParameterApiDescriptionModel
        {
            Name = parameterInfo.Name!,
            TypeAsString = parameterInfo.ParameterType.GetFullNameWithAssemblyName(),
            Type = TypeHelper.GetFullNameHandlingNullableAndGenerics(parameterInfo.ParameterType),
            TypeSimple = ApiTypeNameHelper.GetSimpleTypeName(parameterInfo.ParameterType),
            IsOptional = parameterInfo.IsOptional,
            DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null
        };
    }
}
