using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionPropertyUiDto
{
    public ExtensionPropertyUiTableDto OnTable { get; set; }
    public ExtensionPropertyUiFormDto OnCreateForm { get; set; }
    public ExtensionPropertyUiFormDto OnEditForm { get; set; }
    public ExtensionPropertyUiLookupDto Lookup { get; set; }
}
