using System;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Serializable]
    public class ApplicationLocalizationConfigurationDto
    {
        //TODO: Rename to Texts?
        public Dictionary<string, Dictionary<string, string>> Values { get; set; }

        public List<LanguageInfo> Languages { get; set; }

        public CurrentCultureDto CurrentCulture { get; set; }

        public ApplicationLocalizationConfigurationDto()
        {
            Values = new Dictionary<string, Dictionary<string, string>>();
            Languages = new List<LanguageInfo>();
        }
    }
}