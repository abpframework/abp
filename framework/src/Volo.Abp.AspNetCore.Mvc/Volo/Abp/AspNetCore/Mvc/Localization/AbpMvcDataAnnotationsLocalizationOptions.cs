using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class AbpMvcDataAnnotationsLocalizationOptions
{
    public IDictionary<Assembly, Type> AssemblyResources { get; }

    public AbpMvcDataAnnotationsLocalizationOptions()
    {
        AssemblyResources = new Dictionary<Assembly, Type>();
    }

    public void AddAssemblyResource(
        [NotNull] Type resourceType,
        params Assembly[] assemblies)
    {
        if (assemblies.IsNullOrEmpty())
        {
            assemblies = new[] { resourceType.Assembly };
        }

        foreach (var assembly in assemblies)
        {
            AssemblyResources[assembly] = resourceType;
        }
    }
}
