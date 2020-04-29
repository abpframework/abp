using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleObjectExtensionDto
    {
        public Dictionary<string, ModuleObjectExtraPropertyExtensionDto> ExtraProperties { get; set; }
    }
}