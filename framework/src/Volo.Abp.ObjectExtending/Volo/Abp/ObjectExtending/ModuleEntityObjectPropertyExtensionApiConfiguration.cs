using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ModuleEntityObjectPropertyExtensionApiConfiguration
    {
        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiGetConfiguration OnGet { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiCreateConfiguration OnCreate { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiUpdateConfiguration OnUpdate { get; }

        public ModuleEntityObjectPropertyExtensionApiConfiguration()
        {
            OnGet = new ModuleEntityObjectPropertyExtensionApiGetConfiguration();
            OnCreate = new ModuleEntityObjectPropertyExtensionApiCreateConfiguration();
            OnUpdate = new ModuleEntityObjectPropertyExtensionApiUpdateConfiguration();
        }
    }
}