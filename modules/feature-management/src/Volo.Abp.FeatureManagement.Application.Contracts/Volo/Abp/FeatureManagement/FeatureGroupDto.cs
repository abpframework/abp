using System.Collections.Generic;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureGroupDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<FeatureDto> Features { get; set; }

        public string GetNormalizedGroupName()
        {
            return Name.Replace(".", "_");
        }
    }
}
