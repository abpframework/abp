using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ModuleObjectExtraPropertyUiExtensionDto
    {
        public ModuleObjectExtraPropertyUiTableExtensionDto Table { get; set; }
        public ModuleObjectExtraPropertyUiFormExtensionDto CreateForm { get; set; }
        public ModuleObjectExtraPropertyUiFormExtensionDto EditForm { get; set; }
    }
}