using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch
{
    public class LanguageSwitchViewComponent : AbpViewComponent
    {
        private readonly ILanguageProvider _languageProvider;

        public LanguageSwitchViewComponent(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languageProvider.GetLanguagesAsync();
            var currentLanguage = FindCurrentLanguage(languages);

            var model = new LanguageSwitchViewComponentModel
            {
                CurrentLanguage = currentLanguage,
                OtherLanguages = languages.Where(l => l != currentLanguage).ToList()
            };
            
            return View("~/Themes/Basic/Components/Toolbar/LanguageSwitch/Default.cshtml", model);
        }

        private LanguageInfo FindCurrentLanguage(IReadOnlyList<LanguageInfo> languages)
        {
            return languages.FirstOrDefault(l =>
                       l.CultureName == CultureInfo.CurrentCulture.Name &&
                       l.UiCultureName == CultureInfo.CurrentUICulture.Name)
                   ?? languages.FirstOrDefault(l => l.CultureName == CultureInfo.CurrentCulture.Name)
                   ?? languages.FirstOrDefault(l => l.UiCultureName == CultureInfo.CurrentUICulture.Name);
        }
    }
}
