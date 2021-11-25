using System.Collections.Generic;

namespace Volo.Abp.FeatureManagement;

public class GetFeatureListResultDto
{
    public List<FeatureGroupDto> Groups { get; set; }
}
