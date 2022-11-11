namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationConfigurationRequestOptions
{
    /// <summary>
    /// Set to true to fill the Values property in <see cref="ApplicationConfigurationDto.Localization"/>.
    /// </summary>
    public bool IncludeLocalizationResources { get; set; } = true;
}