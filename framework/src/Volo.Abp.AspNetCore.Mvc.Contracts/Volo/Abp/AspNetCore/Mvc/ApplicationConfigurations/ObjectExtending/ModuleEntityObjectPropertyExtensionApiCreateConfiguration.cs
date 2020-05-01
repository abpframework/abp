using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleEntityObjectPropertyExtensionApiCreateConfigurationDto
    {
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}