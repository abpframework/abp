using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public class AbpApplicationConfigurationAppService : ApplicationService, IAbpApplicationConfigurationAppService
    {
        private readonly AbpLocalizationOptions _localizationOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUser _currentUser;
        private readonly ISettingProvider _settingProvider;
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly IFeatureDefinitionManager _featureDefinitionManager;
        private readonly ILanguageProvider _languageProvider;

        public AbpApplicationConfigurationAppService(
            IOptions<AbpLocalizationOptions> localizationOptions,
            IServiceProvider serviceProvider,
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
            IAuthorizationService authorizationService,
            ICurrentUser currentUser,
            ISettingProvider settingProvider,
            ISettingDefinitionManager settingDefinitionManager,
            IFeatureDefinitionManager featureDefinitionManager,
            ILanguageProvider languageProvider)
        {
            _serviceProvider = serviceProvider;
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _authorizationService = authorizationService;
            _currentUser = currentUser;
            _settingProvider = settingProvider;
            _settingDefinitionManager = settingDefinitionManager;
            _featureDefinitionManager = featureDefinitionManager;
            _languageProvider = languageProvider;
            _localizationOptions = localizationOptions.Value;
        }

        public virtual async Task<ApplicationConfigurationDto> GetAsync()
        {
            //TODO: Optimize & cache..?

            return new ApplicationConfigurationDto
            {
                Auth = await GetAuthConfigAsync(),
                Features = await GetFeaturesConfigAsync(),
                Localization = await GetLocalizationConfigAsync(),
                CurrentUser = GetCurrentUser(),
                Setting = await GetSettingConfigAsync()

            };
        }

        protected virtual CurrentUserDto GetCurrentUser()
        {
            return new CurrentUserDto
            {
                IsAuthenticated = _currentUser.IsAuthenticated,
                Id = _currentUser.Id,
                TenantId = _currentUser.TenantId,
                UserName = _currentUser.UserName
            };
        }

        protected virtual async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
        {
            Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetAuthConfigAsync()");

            var authConfig = new ApplicationAuthConfigurationDto();

            var policyNames = await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync();

            Logger.LogDebug($"GetPoliciesNamesAsync returns {policyNames.Count} items.");

            foreach (var policyName in policyNames)
            {
                authConfig.Policies[policyName] = true;

                Logger.LogDebug($"_authorizationService.IsGrantedAsync? {policyName}");

                if (await _authorizationService.IsGrantedAsync(policyName))
                {
                    authConfig.GrantedPolicies[policyName] = true;
                }
            }

            Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetAuthConfigAsync()");

            return authConfig;
        }

        protected virtual async Task<ApplicationLocalizationConfigurationDto> GetLocalizationConfigAsync()
        {
            Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetLocalizationConfigAsync()");

            var localizationConfig = new ApplicationLocalizationConfigurationDto();

            localizationConfig.Languages.AddRange(await _languageProvider.GetLanguagesAsync());

            foreach (var resource in _localizationOptions.Resources.Values)
            {
                var dictionary = new Dictionary<string, string>();

                var localizer = _serviceProvider.GetRequiredService(
                    typeof(IStringLocalizer<>).MakeGenericType(resource.ResourceType)
                ) as IStringLocalizer;

                foreach (var localizedString in localizer.GetAllStrings())
                {
                    dictionary[localizedString.Name] = localizedString.Value;
                }

                localizationConfig.Values[resource.ResourceName] = dictionary;
            }

            localizationConfig.CurrentCulture = GetCurrentCultureInfo();

            Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetLocalizationConfigAsync()");

            return localizationConfig;
        }

        private static CurrentCultureDto GetCurrentCultureInfo()
        {
            return new CurrentCultureDto
            {
                Name = CultureInfo.CurrentUICulture.Name,
                DisplayName = CultureInfo.CurrentUICulture.DisplayName,
                EnglishName = CultureInfo.CurrentUICulture.EnglishName,
                NativeName = CultureInfo.CurrentUICulture.NativeName,
                IsRightToLeft = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
                CultureName = CultureInfo.CurrentUICulture.TextInfo.CultureName,
                TwoLetterIsoLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                ThreeLetterIsoLanguageName = CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName,
                DateTimeFormat = new DateTimeFormatDto
                {
                    CalendarAlgorithmType = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType.ToString(),
                    DateTimeFormatLong = CultureInfo.CurrentUICulture.DateTimeFormat.LongDatePattern,
                    ShortDatePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                    FullDateTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern,
                    DateSeparator = CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator,
                    ShortTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern,
                    LongTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern,
                }
            };
        }

        private async Task<ApplicationSettingConfigurationDto> GetSettingConfigAsync()
        {
            Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetSettingConfigAsync()");

            var result = new ApplicationSettingConfigurationDto
            {
                Values = new Dictionary<string, string>()
            };

            foreach (var settingDefinition in _settingDefinitionManager.GetAll())
            {
                if (!settingDefinition.IsVisibleToClients)
                {
                    continue;
                }

                result.Values[settingDefinition.Name] = await _settingProvider.GetOrNullAsync(settingDefinition.Name);
            }

            Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetSettingConfigAsync()");

            return result;
        }

        protected virtual async Task<ApplicationFeatureConfigurationDto> GetFeaturesConfigAsync()
        {
            Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetFeaturesConfigAsync()");

            var result = new ApplicationFeatureConfigurationDto();

            foreach (var featureDefinition in _featureDefinitionManager.GetAll())
            {
                if (!featureDefinition.IsVisibleToClients)
                {
                    continue;
                }

                result.Values[featureDefinition.Name] = await FeatureChecker.GetOrNullAsync(featureDefinition.Name);
            }

            Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetFeaturesConfigAsync()");

            return result;
        }
    }
}