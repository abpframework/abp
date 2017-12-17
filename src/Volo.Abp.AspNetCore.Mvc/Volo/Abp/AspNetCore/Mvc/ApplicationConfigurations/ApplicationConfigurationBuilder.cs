using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public class ApplicationConfigurationBuilder : IApplicationConfigurationBuilder, ITransientDependency
    {
        private readonly AbpLocalizationOptions _localizationOptions;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationConfigurationBuilder(IOptions<AbpLocalizationOptions> localizationOptions, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _localizationOptions = localizationOptions.Value;
        }

        public Task<ApplicationConfigurationDto> Get()
        {
            var config = new ApplicationConfigurationDto
            {
                Localization = GetLocalizationConfig()
            };

            return Task.FromResult(config);
        }

        private ApplicationLocalizationConfigurationDto GetLocalizationConfig()
        {
            //TODO: Optimize & cache

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