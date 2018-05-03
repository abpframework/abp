using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Views.Shared.Components.Theme.MainNavbar.Toolbar.Items.LanguageSwitch
{
    public class LanguageSwitchViewComponent : AbpViewComponent
    {
        private readonly RequestLocalizationOptions _options;

        public LanguageSwitchViewComponent(IOptions<RequestLocalizationOptions> options)
        {
            _options = options.Value;
        }

        public IViewComponentResult Invoke()
        {
            //TODO: Better handle culture & uiculture separation!

            var model = new LanguageSwitchViewComponentModel
            {
                CurrentLanguage = new LanguageInfo
                {
                    Name = CultureInfo.CurrentUICulture.Name,
                    DisplayName = CultureInfo.CurrentUICulture.DisplayName,
                    Icon = null //TODO!
                }
            };

            foreach (var supportedUiCulture in _options.SupportedUICultures)
            {
                if (model.CurrentLanguage.Name == supportedUiCulture.Name)
                {
                    continue;
                }

                model.OtherLanguages.Add(new LanguageInfo
                {
                    Name = supportedUiCulture.Name,
                    DisplayName = supportedUiCulture.DisplayName,
                    Icon = null //TODO!
                });
            }

            return View("~/Views/Shared/Components/Theme/MainNavbar/Toolbar/Items/LanguageSwitch/Default.cshtml", model);
        }
    }
}
