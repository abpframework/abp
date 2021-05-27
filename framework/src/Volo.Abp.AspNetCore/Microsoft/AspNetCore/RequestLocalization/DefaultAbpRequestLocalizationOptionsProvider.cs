using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public class DefaultAbpRequestLocalizationOptionsProvider : IAbpRequestLocalizationOptionsProvider, ITransientDependency
    {
        protected readonly IServiceScopeFactory ServiceProviderFactory;

        public DefaultAbpRequestLocalizationOptionsProvider(IServiceScopeFactory serviceProviderFactory)
        {
            ServiceProviderFactory = serviceProviderFactory;
        }

        public async Task<RequestLocalizationOptions> GetLocalizationOptionsAsync()
        {
            using (var serviceScope = ServiceProviderFactory.CreateScope())
            {
                var languageProvider = serviceScope.ServiceProvider.GetRequiredService<ILanguageProvider>();
                var settingProvider = serviceScope.ServiceProvider.GetRequiredService<ISettingProvider>();

                var languages = await languageProvider.GetLanguagesAsync();
                var defaultLanguage = await settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);

                var options = !languages.Any()
                    ? new RequestLocalizationOptions()
                    : new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = await GetDefaultRequestCultureAsync(defaultLanguage, languages),

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

                foreach (var configurator in serviceScope.ServiceProvider
                    .GetRequiredService<IOptions<AbpRequestLocalizationOptions>>()
                    .Value.RequestLocalizationOptionConfigurators)
                {
                    await configurator(serviceScope.ServiceProvider, options);
                }

                return options;
            }
        }

        protected virtual Task<RequestCulture> GetDefaultRequestCultureAsync(string defaultLanguage, IReadOnlyList<LanguageInfo> languages)
        {
            if (defaultLanguage == null)
            {
                var firstLanguage = languages[0];
                return Task.FromResult(new RequestCulture(firstLanguage.CultureName, firstLanguage.UiCultureName));
            }

            var (cultureName, uiCultureName) = LocalizationSettingHelper.ParseLanguageSetting(defaultLanguage);
            return Task.FromResult(new RequestCulture(cultureName, uiCultureName));
        }
    }
}
