using System.Collections.Generic;

namespace Volo.Abp.FeatureManagement;

public class UpdateFeaturesDto
{
    public List<UpdateFeatureDto> Features { get; set; }
}
