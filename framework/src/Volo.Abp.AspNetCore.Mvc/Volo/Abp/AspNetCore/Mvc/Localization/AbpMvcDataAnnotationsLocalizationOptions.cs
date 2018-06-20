using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class AbpMvcDataAnnotationsLocalizationOptions
    {
        public IDictionary<Assembly, Type> AssemblyResources { get; }

        public AbpMvcDataAnnotationsLocalizationOptions()
        {
            AssemblyResources = new Dictionary<Assembly, Type>();
        }

        public void AddAssemblyResource([NotNull] Type resourceType, [CanBeNull] Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = resourceType.Assembly;
            }

            AssemblyResources[assembly] = resourceType;
        }
    }
}