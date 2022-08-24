using System.Collections.Generic;

namespace Volo.Abp.Localization.Distributed;

public class DistributedLocalizationResourceData
{
    public string ResourceName { get; }

    public Dictionary<string, string> Texts { get; }

    public DistributedLocalizationResourceData(string resourceName)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        Texts = new();
    }

    public DistributedLocalizationResourceData(string resourceName, Dictionary<string, string> texts)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        Texts = Check.NotNull(texts, nameof(texts));
    }
}