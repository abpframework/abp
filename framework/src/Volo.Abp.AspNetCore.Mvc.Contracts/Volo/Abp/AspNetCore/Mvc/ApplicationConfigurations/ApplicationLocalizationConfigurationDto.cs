using System;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationLocalizationConfigurationDto
{
    /// <summary>
    /// This is not filled if <see cref="ApplicationConfigurationRequestOptions.IncludeLocalizationResources"/> is false.
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> Values { get; set; }

    /// <summary>
    /// This property will never be filled by the application configuration endpoint
    /// (by AbpApplicationConfigurationAppService). However, it is here to be filled
    /// using the application localization endpoint (AbpApplicationLocalizationAppService).
    /// This is an ugly design, but it is the best solution for backward-compability and
    /// simple implementation.
    /// 
    /// It's client's responsibility to fill this property
    /// using the application localization endpoint.
    /// </summary>
    public Dictionary<string, ApplicationLocalizationResourceDto> Resources { get; set; } = new();

    public List<LanguageInfo> Languages { get; set; }

    public CurrentCultureDto CurrentCulture { get; set; }

    public string? DefaultResourceName { get; set; }

    public Dictionary<string, List<NameValue>> LanguagesMap { get; set; }

    public Dictionary<string, List<NameValue>> LanguageFilesMap { get; set; }

    public ApplicationLocalizationConfigurationDto()
    {
        Values = new Dictionary<string, Dictionary<string, string>>();
        Languages = new List<LanguageInfo>();
        CurrentCulture = new CurrentCultureDto();
        LanguagesMap = new Dictionary<string, List<NameValue>>();
        LanguageFilesMap = new Dictionary<string, List<NameValue>>();
    }
}
