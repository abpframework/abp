using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch
{
    public class LanguageSwitchViewComponentModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public List<LanguageInfo> OtherLanguages { get; }

        public LanguageSwitchViewComponentModel()
        {
            OtherLanguages = new List<LanguageInfo>();
        }
    }
}