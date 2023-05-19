namespace LocalizationKeySynchronizer;

public class AbpLocalization
{
    public AbpLocalization(string filePath, AbpLocalizationInfo localizationInfo)
    {
        FilePath = filePath;
        LocalizationInfo = localizationInfo;
    }

    public string FilePath { get; set; }

    public AbpLocalizationInfo LocalizationInfo { get; set; }
}