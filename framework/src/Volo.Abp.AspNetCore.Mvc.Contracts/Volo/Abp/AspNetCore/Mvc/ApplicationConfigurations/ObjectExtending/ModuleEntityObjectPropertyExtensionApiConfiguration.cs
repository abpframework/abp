using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    public class ModuleEntityObjectPropertyExtensionApiConfigurationDto
    {
        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiGetConfigurationDto OnGet { get; set; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiCreateConfigurationDto OnCreate { get; set; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiUpdateConfigurationDto OnUpdate { get; set; }

        public ModuleEntityObjectPropertyExtensionApiConfigurationDto()
        {
            OnGet = new ModuleEntityObjectPropertyExtensionApiGetConfigurationDto();
            OnCreate = new ModuleEntityObjectPropertyExtensionApiCreateConfigurationDto();
            OnUpdate = new ModuleEntityObjectPropertyExtensionApiUpdateConfigurationDto();
        }
    }
}