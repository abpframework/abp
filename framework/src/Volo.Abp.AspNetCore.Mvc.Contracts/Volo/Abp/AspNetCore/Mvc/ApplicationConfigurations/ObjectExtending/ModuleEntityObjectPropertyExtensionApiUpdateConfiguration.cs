using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleEntityObjectPropertyExtensionApiUpdateConfigurationDto
    {
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}