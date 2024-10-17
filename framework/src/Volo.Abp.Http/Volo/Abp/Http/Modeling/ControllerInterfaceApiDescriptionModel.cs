using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class ControllerInterfaceApiDescriptionModel
{
    public string Type { get; set; } = default!;

    public string Name { get; set; } = default!;

    public InterfaceMethodApiDescriptionModel[] Methods { get; set; } = default!;

    public ControllerInterfaceApiDescriptionModel()
    {

    }

    public static ControllerInterfaceApiDescriptionModel Create(Type type)
    {
        var model = new ControllerInterfaceApiDescriptionModel
        {
            Type = type.FullName!,
            Name = type.Name
        };

        var methods = new List<InterfaceMethodApiDescriptionModel>();

        var methodInfos = new List<MethodInfo>();
        foreach (var methodInfo in type.GetMethods())
        {
            methodInfos.Add(methodInfo);
            methods.Add(InterfaceMethodApiDescriptionModel.Create(methodInfo));
        }

        foreach (var @interface in type.GetInterfaces())
        {
            foreach (var method in @interface.GetMethods())
            {
                if (!methodInfos.Contains(method))
                {
                    methods.Add(InterfaceMethodApiDescriptionModel.Create(method));
                }
                methodInfos.Add(method);
            }
        }

        model.Methods = methods.ToArray();
        return model;
    }
}
