using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleObjectExtraPropertyExtensionDto
    {
        public string Type { get; set; }

        public string TypeSimple { get; set; }

        public LocalizedDisplayNameDto DisplayName { get; set; }

        public ModuleObjectExtraPropertyUiExtensionDto Ui { get; set; }

        public List<ModuleObjectExtraPropertyAttributeDto> Attributes { get; set; }
    }
}