using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class AbpApplicationLocalizationAppService :
    ApplicationService,
    IAbpApplicationLocalizationAppService
{
    protected IExternalLocalizationStore ExternalLocalizationStore { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected AbpRequestCultureResolver RequestCultureResolver { get; }

    public AbpApplicationLocalizationAppService(
        IExternalLocalizationStore externalLocalizationStore,
        IOptions<AbpLocalizationOptions> localizationOptions,
        AbpRequestCultureResolver requestCultureResolver)
    {
        ExternalLocalizationStore = externalLocalizationStore;
        LocalizationOptions = localizationOptions.Value;
        RequestCultureResolver = requestCultureResolver;
    }

    public virtual async Task<ApplicationLocalizationDto> GetAsync(ApplicationLocalizationRequestDto input)
    {
        var culture = await RequestCultureResolver.ResolveAsync(input.CultureName);
        if (culture == null)
        {
            throw new AbpValidationException(new[]
            {
                new ValidationResult("The selected culture is not supported.", new[] { nameof(input.CultureName) })
            });
        }

        using (CultureHelper.Use(culture))
        {
            var resources = LocalizationOptions
                .Resources
                .Values
                .Union(
                    await ExternalLocalizationStore.GetResourcesAsync()
                ).ToArray();

            var localizationConfig = new ApplicationLocalizationDto
            {
                Resources = new Dictionary<string, ApplicationLocalizationResourceDto>(resources.Length),
                CurrentCulture = CurrentCultureDto.Create()
            };

            foreach (var resource in resources)
            {
                var dictionary = new Dictionary<string, string>();
                var localizer = await StringLocalizerFactory.CreateByResourceNameOrNullAsync(resource.ResourceName);
                if (localizer != null)
                {
                    Dictionary<string, LocalizedString>? staticLocalizedStrings = null;

                    if (input.OnlyDynamics)
                    {
                        staticLocalizedStrings = (await localizer.GetAllStringsAsync(
                            includeParentCultures: true,
                            includeBaseLocalizers: false,
                            includeDynamicContributors: false
                        )).ToDictionary(x => x.Name);
                    }

                    var localizedStringsWithDynamics = await localizer.GetAllStringsAsync(
                        includeParentCultures: true,
                        includeBaseLocalizers: false,
                        includeDynamicContributors: true
                    );

                    foreach (var localizedString in localizedStringsWithDynamics)
                    {
                        if (input.OnlyDynamics)
                        {
                            var staticLocalizedString = staticLocalizedStrings!.GetOrDefault(localizedString.Name);
                            if (staticLocalizedString != null &&
                                localizedString.Value == staticLocalizedString.Value)
                            {
                                continue;
                            }
                        }

                        dictionary[localizedString.Name] = localizedString.Value;
                    }
                }

                localizationConfig.Resources[resource.ResourceName] =
                    new ApplicationLocalizationResourceDto {
                        Texts = dictionary,
                        BaseResources = resource.BaseResourceNames.ToArray()
                    };
            }

            return localizationConfig;
        }
    }
}
