using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
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

        public AbpApplicationConfigurationAppService(
            IOptions<AbpLocalizationOptions> localizationOptions,
            IServiceProvider serviceProvider,
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
            IAuthorizationService authorizationService,
            ICurrentUser currentUser)
        {
            _serviceProvider = serviceProvider;
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _authorizationService = authorizationService;
            _currentUser = currentUser;
            _localizationOptions = localizationOptions.Value;
        }

        public async Task<ApplicationConfigurationDto> GetAsync()
        {
            //TODO: Optimize & cache..?

            return new ApplicationConfigurationDto
            {
                Auth = await GetAuthConfig(),
                Localization = GetLocalizationConfig(),
                CurrentUser = GetCurrentUser()
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

        protected virtual async Task<ApplicationAuthConfigurationDto> GetAuthConfig()
        {
            var authConfig = new ApplicationAuthConfigurationDto();

            foreach (var policyName in await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync())
            {
                authConfig.Policies[policyName] = true;

                if (await _authorizationService.IsGrantedAsync(policyName))
                {
                    authConfig.GrantedPolicies[policyName] = true;
                }
            }

            return authConfig;
        }

        protected virtual ApplicationLocalizationConfigurationDto GetLocalizationConfig()
        {
            var localizationConfig = new ApplicationLocalizationConfigurationDto();

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

                var resourceName = LocalizationResourceNameAttribute.GetName(resource.ResourceType);
                localizationConfig.Values[resourceName] = dictionary;
            }

            return localizationConfig;
        }
    }
}