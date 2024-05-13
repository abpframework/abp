using System.Linq;
using System.Reflection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public static class WebAppAdditionalAssembliesHelper
{
    public static Assembly[] GetAssemblies<TModule>()
        where TModule : IAbpModule
    {
        return AbpModuleHelper.FindAllModuleTypes(typeof(TModule), null)
            .Where(t => t.Name.Contains("Blazor") || t.Name.Contains("WebAssembly"))
            .Select(t => t.Assembly)
            .Distinct()
            .ToArray();
    }
}
