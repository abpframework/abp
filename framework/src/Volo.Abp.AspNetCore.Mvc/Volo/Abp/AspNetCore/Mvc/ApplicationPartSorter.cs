using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc;

/// <summary>
/// This class is used to align order of the MVC Application Parts with the order of
/// ABP module dependencies.
/// </summary>
public static class ApplicationPartSorter
{
    public static void Sort(ApplicationPartManager partManager, IModuleContainer moduleContainer)
    {
        var orderedModuleAssemblies = moduleContainer.Modules
            .Select((moduleDescriptor, index) => new { moduleDescriptor.Assembly, index })
            .ToDictionary(x => x.Assembly, x => x.index);

        var modulesAssemblies = moduleContainer.Modules.Select(x => x.Assembly).ToList();
        var sortedTypes = partManager.ApplicationParts
            .Where(x => modulesAssemblies.Contains(GetApplicationPartAssembly(x)))
            .OrderBy(x => orderedModuleAssemblies[GetApplicationPartAssembly(x)])
            .ToList();

        var sortIndex = 0;
        var sortedParts = partManager.ApplicationParts
            .Select(x => modulesAssemblies.Contains(GetApplicationPartAssembly(x)) ? sortedTypes[sortIndex++] : x)
            .ToList();

        partManager.ApplicationParts.Clear();
        sortedParts.Reverse();
        foreach (var applicationPart in sortedParts)
        {
            partManager.ApplicationParts.Add(applicationPart);
        }
    }

    private static Assembly GetApplicationPartAssembly(ApplicationPart part)
    {
        return part switch
        {
            AssemblyPart assemblyPart => assemblyPart.Assembly,
            CompiledRazorAssemblyPart compiledRazorAssemblyPart => compiledRazorAssemblyPart.Assembly,
            _ => throw new AbpException("Unknown application part type")
        };
    }
}
