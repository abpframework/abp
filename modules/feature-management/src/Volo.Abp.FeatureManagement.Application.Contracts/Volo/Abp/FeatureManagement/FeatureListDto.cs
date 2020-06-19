using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureListDto : ExtensibleObject
    {
        public List<FeatureDto> Features { get; set; }
    }
}