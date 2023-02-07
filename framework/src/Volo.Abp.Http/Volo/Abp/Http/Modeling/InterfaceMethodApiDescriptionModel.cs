using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class InterfaceMethodApiDescriptionModel
{
    public string Name { get; set; }

    public IList<MethodParameterApiDescriptionModel> ParametersOnMethod { get; set; }

    public ReturnValueApiDescriptionModel ReturnValue { get; set; }

    public InterfaceMethodApiDescriptionModel()
    {

    }

    public static InterfaceMethodApiDescriptionModel Create([NotNull] MethodInfo method)
    {
        return new InterfaceMethodApiDescriptionModel
        {
            Name = method.Name,
            ReturnValue = ReturnValueApiDescriptionModel.Create(method.ReturnType),
            ParametersOnMethod = method
                .GetParameters()
                .Select(MethodParameterApiDescriptionModel.Create)
                .ToList(),
        };
    }

    public override string ToString()
    {
        return $"[InterfaceMethodApiDescriptionModel {Name}]";
    }
}
