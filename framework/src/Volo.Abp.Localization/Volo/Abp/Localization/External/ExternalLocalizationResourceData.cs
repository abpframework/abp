using System.Collections.Generic;

namespace Volo.Abp.Localization.Distributed;

public class ExternalLocalizationResourceData
{
    public string ResourceName { get; }

    public Dictionary<string, string> Texts { get; }

    public ExternalLocalizationResourceData(string resourceName)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        Texts = new();
    }

    public ExternalLocalizationResourceData(string resourceName, Dictionary<string, string> texts)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        Texts = Check.NotNull(texts, nameof(texts));
    }
}