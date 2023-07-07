using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Volo.Abp.AspNetCore.Mvc;

public class AbpAspNetCoreMvcOptions
{
    public bool? MinifyGeneratedScript { get; set; }

    public AbpConventionalControllerOptions ConventionalControllers { get; }

    public HashSet<Type> IgnoredControllersOnModelExclusion { get; }

    public HashSet<Type> ControllersToRemove { get; }

    public bool ExposeIntegrationServices { get; set; } = false;

    public bool AutoModelValidation { get; set; }

    public bool EnableRazorRuntimeCompilationOnDevelopment { get; set; }

    public bool ChangeControllerModelApiExplorerGroupName { get; set; }

    public AbpAspNetCoreMvcOptions()
    {
        ConventionalControllers = new AbpConventionalControllerOptions();
        IgnoredControllersOnModelExclusion = new HashSet<Type>();
        ControllersToRemove = new HashSet<Type>();
        AutoModelValidation = true;
        EnableRazorRuntimeCompilationOnDevelopment = true;
        ChangeControllerModelApiExplorerGroupName = true;
    }
}
