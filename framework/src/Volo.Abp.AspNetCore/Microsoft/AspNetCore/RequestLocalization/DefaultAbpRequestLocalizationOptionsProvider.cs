using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public class DefaultAbpRequestLocalizationOptionsProvider : IAbpRequestLocalizationOptionsProvider, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private Lazy<RequestLocalizationOptions> _lazyRequestLocalizationOptions;

        public DefaultAbpRequestLocalizationOptionsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void InitLocalizationOptions(Action<RequestLocalizationOptions> optionsAction = null)
        {
            _lazyRequestLocalizationOptions = new Lazy<RequestLocalizationOptions>(() =>
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var languageProvider = serviceScope.ServiceProvider.GetRequiredService<ILanguageProvider>();
                    var settingProvider = serviceScope.ServiceProvider.GetRequiredService<ISettingProvider>();

                    var languages = AsyncHelper.RunSync(languageProvider.GetLanguagesAsync);
                    var defaultLanguage = AsyncHelper.RunSync(() =>
                        settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage));

                    var options = !languages.Any()
                        ? new RequestLocalizationOptions()
                        : new RequestLocalizationOptions
                        {
                            DefaultRequestCulture = DefaultGetRequestCulture(defaultLanguage, languages),

                            SupportedCultures = languages
                                .Select(l => l.CultureName)
                                .Distinct()
                                .Select(c => new CultureInfo(c))
                                .ToArray(),

                            SupportedUICultures = languages
                                .Select(l => l.UiCultureName)
                                .Distinct()
                                .Select(c => new CultureInfo(c))
                                .ToArray()
                        };

                    optionsAction?.Invoke(options);
                    return options;
                }
            }, true);
        }

        public RequestLocalizationOptions GetLocalizationOptions()
        {
            return _lazyRequestLocalizationOptions.Value;
        }

        private static RequestCulture DefaultGetRequestCulture(string defaultLanguage, IReadOnlyList<LanguageInfo> languages)
        {
            if (defaultLanguage == null)
            {
                var firstLanguage = languages.First();
                return new RequestCulture(firstLanguage.CultureName, firstLanguage.UiCultureName);
            }

            var (cultureName, uiCultureName) = LocalizationSettingHelper.ParseLanguageSetting(defaultLanguage);
            return new RequestCulture(cultureName, uiCultureName);
        }
    }
}