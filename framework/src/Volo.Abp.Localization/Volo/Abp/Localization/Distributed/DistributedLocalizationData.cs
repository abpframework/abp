using System.Collections.Generic;

namespace Volo.Abp.Localization.Distributed;

public class DistributedLocalizationData
{
    public List<DistributedLocalizationResourceData> Resources { get; }

    public DistributedLocalizationData()
    {
        Resources = new();
    }
    
    public DistributedLocalizationData(List<DistributedLocalizationResourceData> resources)
    {
        Resources = Check.NotNull(resources, nameof(resources));
    }
}