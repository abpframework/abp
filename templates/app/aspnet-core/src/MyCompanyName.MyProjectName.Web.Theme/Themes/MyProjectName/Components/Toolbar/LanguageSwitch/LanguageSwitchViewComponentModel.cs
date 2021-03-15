using System.Collections.Generic;
using Volo.Abp.Localization;

namespace MyCompanyName.MyProjectName.Web.Theme.Themes.MyProjectName.Components.Toolbar.LanguageSwitch
{
    public class LanguageSwitchViewComponentModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public List<LanguageInfo> OtherLanguages { get; set; }
    }
}