using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class ExtensionPropertyDto
    {
        public string Type { get; set; }

        public string TypeSimple { get; set; }

        [CanBeNull]
        public LocalizableStringDto DisplayName { get; set; }

        public ExtensionPropertyApiDto Api { get; set; }
     
        public ExtensionPropertyUiDto Ui { get; set; }

        public List<ExtensionPropertyAttributeDto> Attributes { get; set; }

        public Dictionary<string, object> Configuration { get; set; }
        
        public object DefaultValue { get; set; }
    }
}