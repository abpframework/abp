using System.Collections.Generic;

namespace LocalizationKeySynchronizer;

public class AbpAsyncLocalizationViewModel
{
    public AbpAsyncLocalizationViewModel(string referenceCulture, string culture, string path, List<AbpAsyncKey> asyncKeys)
    {
        ReferenceCulture = referenceCulture;
        Culture = culture;
        Path = path;
        AsyncKeys = asyncKeys;
    }

    public string ReferenceCulture { get; set; }
    public string Culture { get; set; }
    public string Path { get; set; }

    public List<AbpAsyncKey> AsyncKeys { get; set; }
}