using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleExtensionDto
    {
        public Dictionary<string, ModuleObjectExtensionDto> Objects { get; set; }
    }
}

