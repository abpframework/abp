namespace Volo.Abp.Localization.Distributed;

public class AbpDistributedLocalizationOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveToDistributedStore { get; set; } = true;
    
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool GetFromDistributedStore { get; set; }
}