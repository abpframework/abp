using System.Collections.Generic;

namespace LocalizationKeySynchronizer;

public class AbpAsyncLocalization
{
    public AbpAsyncLocalization(AbpLocalization localization, AbpLocalization reference, List<AbpAsyncKey> asyncKeys)
    {
        Localization = localization;
        Reference = reference;
        AsyncKeys = asyncKeys;
    }

    public AbpLocalization Localization { get; set; }
    public AbpLocalization Reference { get; set; }

    public List<AbpAsyncKey> AsyncKeys { get; set; }
}