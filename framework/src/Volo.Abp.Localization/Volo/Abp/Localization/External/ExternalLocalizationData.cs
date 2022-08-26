using System.Collections.Generic;

namespace Volo.Abp.Localization.External;

public class ExternalLocalizationData
{
    public List<ExternalLocalizationResourceData> Resources { get; }

    public ExternalLocalizationData()
    {
        Resources = new();
    }
    
    public ExternalLocalizationData(List<ExternalLocalizationResourceData> resources)
    {
        Resources = Check.NotNull(resources, nameof(resources));
    }
}