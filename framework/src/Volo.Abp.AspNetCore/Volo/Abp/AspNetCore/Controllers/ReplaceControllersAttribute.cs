using System;

namespace Volo.Abp.AspNetCore.Controllers;

[AttributeUsage(AttributeTargets.Class)]
public class ReplaceControllersAttribute : Attribute
{
    public Type[] ControllerTypes { get; }

    public ReplaceControllersAttribute(params Type[] controllerTypes)
    {
        ControllerTypes = controllerTypes ?? Type.EmptyTypes;
    }
}
