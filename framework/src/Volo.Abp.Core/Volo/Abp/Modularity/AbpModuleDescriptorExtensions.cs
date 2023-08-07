using System;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Modularity;

public static class AbpModuleDescriptorExtensions
{
    public static Assembly[] GetAdditionalAssemblies(this IAbpModuleDescriptor module)
    {
        return module.AllAssemblies.Length <= 1
            ? Array.Empty<Assembly>()
            : module.AllAssemblies.Where(x => x != module.Assembly).ToArray();
    }
}