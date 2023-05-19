using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity;

public class ExtensionPropertyApiConfiguration
{
    [NotNull]
    public ExtensionPropertyApiGetConfiguration OnGet { get; }

    [NotNull]
    public ExtensionPropertyApiCreateConfiguration OnCreate { get; }

    [NotNull]
    public ExtensionPropertyApiUpdateConfiguration OnUpdate { get; }

    public ExtensionPropertyApiConfiguration()
    {
        OnGet = new ExtensionPropertyApiGetConfiguration();
        OnCreate = new ExtensionPropertyApiCreateConfiguration();
        OnUpdate = new ExtensionPropertyApiUpdateConfiguration();
    }
}
