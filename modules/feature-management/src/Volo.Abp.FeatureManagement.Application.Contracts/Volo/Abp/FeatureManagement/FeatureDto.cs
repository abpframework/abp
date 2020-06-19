using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureDto : ExtensibleObject
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public IStringValueType ValueType { get; set; }

        public int Depth { get; set; }

        public string ParentName { get; set; }
    }
}