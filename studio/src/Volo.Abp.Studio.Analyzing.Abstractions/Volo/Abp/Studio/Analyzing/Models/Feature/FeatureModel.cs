using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Feature;

[PackageContentItemName(ContentTypeName)]
public class FeatureModel : PackageContentItemModel
{
    public const string ContentTypeName = "feature";

    public string ValueType { get; }

    public string DefaultValue { get; }

    public string DisplayName { get; }

    public string Description { get; }

    public bool IsAvailableToHost { get; }

    public bool IsVisibleToClients { get; }

    public FeatureModel(
        [NotNull] string name,
        [NotNull] string valueType,
        string defaultValue,
        string displayName,
        string description,
        bool isAvailableToHost,
        bool isVisibleToClients
        ) : base(name)
    {
        ValueType = valueType;
        DefaultValue = defaultValue;
        DisplayName = displayName;
        Description = description;
        IsAvailableToHost = isAvailableToHost;
        IsVisibleToClients = isVisibleToClients;
    }
}
