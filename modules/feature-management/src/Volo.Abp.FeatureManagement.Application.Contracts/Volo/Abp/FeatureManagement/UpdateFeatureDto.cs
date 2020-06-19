using Volo.Abp.ObjectExtending;

namespace Volo.Abp.FeatureManagement
{
    public class UpdateFeatureDto : ExtensibleObject
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
