using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public class ApplicationConfigurationBuilder : IApplicationConfigurationBuilder, ITransientDependency
    {
        private readonly AbpLocalizationOptions _localizationOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
        private readonly IAuthorizationService _authorizationService;

        public ApplicationConfigurationBuilder(
            IOptions<AbpLocalizationOptions> localizationOptions,
            IServiceProvider serviceProvider,
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
            IAuthorizationService authorizationService)
        {
            _serviceProvider = serviceProvider;
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _authorizationService = authorizationService;
            _localizationOptions = localizationOptions.Value;
        }

        public async Task<ApplicationConfigurationDto> GetAsync() //TODO: Test, at least with a simple Get
        {
            //TODO: Optimize & cache..?

            return new ApplicationConfigurationDto
            {
                Auth = await GetAuthConfig(),
                Localization = GetLocalizationConfig()
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

                var localizer = _serviceProvider.GetRequiredService(typeof(IStringLocalizer<>).MakeGenericType(resource.ResourceType)) as IStringLocalizer;

                foreach (var localizedString in localizer.GetAllStrings())
                {
                    dictionary[localizedString.Name] = localizedString.Value;
                }

                var resourceShortName = resource.ResourceType
                                            .GetCustomAttributes(true)
                                            .OfType<ShortLocalizationResourceNameAttribute>()
                                            .FirstOrDefault()?.Name ?? resource.ResourceType.FullName;

                localizationConfig.Values[resourceShortName] = dictionary;
            }

            return localizationConfig;
        }
    }
}