using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.FeatureManagement;

public class FeatureDto
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string DisplayNameKey { get; set; }

    public string DisplayNameResource { get; set; }

    public string Value { get; set; }

    public FeatureProviderDto Provider { get; set; }

    public string Description { get; set; }

    public string DescriptionKey { get; set; }

    public string DescriptionResource { get; set; }

    public IStringValueType ValueType { get; set; }

    public int Depth { get; set; }

    public string ParentName { get; set; }
}
