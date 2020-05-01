using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleObjectExtraPropertyUiExtensionDto
    {
        public ModuleObjectExtraPropertyUiTableExtensionDto OnTable { get; set; }
        public ModuleObjectExtraPropertyUiFormExtensionDto OnCreateForm { get; set; }
        public ModuleObjectExtraPropertyUiFormExtensionDto OnEditForm { get; set; }
    }
}