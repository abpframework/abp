using System.Reflection;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Routing;

public class AbpRouterOptions
{
    public Assembly AppAssembly { get; set; } = default!;

    public RouterAssemblyList AdditionalAssemblies { get; }

    public AbpRouterOptions()
    {
        AdditionalAssemblies = new RouterAssemblyList();
    }
}
