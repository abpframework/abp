using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.FeatureManagement
{
    public class UpdateFeaturesDto : ExtensibleObject
    {
        public List<UpdateFeatureDto> Features { get; set; }
    }
}