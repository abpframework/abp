namespace Volo.Abp.Localization.External;

public class AbpExternalLocalizationOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveToExternalStore { get; set; } = true;
    
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool GetFromExternalStore { get; set; }
}