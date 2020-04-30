using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleObjectExtraPropertyExtensionDto
    {
        public string Type { get; set; }

        public string TypeSimple { get; set; }

        [CanBeNull]
        public LocalizableStringDto DisplayName { get; set; }

        public ModuleObjectExtraPropertyUiExtensionDto Ui { get; set; }

        public List<ModuleObjectExtraPropertyAttributeDto> Attributes { get; set; }
    }
}