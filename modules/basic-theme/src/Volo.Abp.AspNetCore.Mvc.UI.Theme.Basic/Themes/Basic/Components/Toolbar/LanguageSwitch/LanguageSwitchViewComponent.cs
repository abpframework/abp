using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
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
            var currentLanguage = languages.FindByCulture(
                CultureInfo.CurrentCulture.Name,
                CultureInfo.CurrentUICulture.Name
            );

            if (currentLanguage == null)
            {
                var abpRequestLocalizationOptionsProvider = HttpContext.RequestServices.GetRequiredService<IAbpRequestLocalizationOptionsProvider>();
                var localizationOptions = await abpRequestLocalizationOptionsProvider.GetLocalizationOptionsAsync();
                if (localizationOptions.DefaultRequestCulture != null)
                {
                    currentLanguage = new LanguageInfo(
                        localizationOptions.DefaultRequestCulture.Culture.Name,
                        localizationOptions.DefaultRequestCulture.UICulture.Name,
                        localizationOptions.DefaultRequestCulture.UICulture.DisplayName);
                }
                else
                {
                    currentLanguage = new LanguageInfo(
                        CultureInfo.CurrentCulture.Name,
                        CultureInfo.CurrentUICulture.Name,
                        CultureInfo.CurrentUICulture.DisplayName);
                }
            }

            var model = new LanguageSwitchViewComponentModel
            {
                CurrentLanguage = currentLanguage,
                OtherLanguages = languages.Where(l => l != currentLanguage).ToList()
            };

            return View("~/Themes/Basic/Components/Toolbar/LanguageSwitch/Default.cshtml", model);
        }
    }
}
