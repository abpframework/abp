using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Authorization;

[PackageContentItemName(ContentTypeName)]
public class PermissionModel : PackageContentItemModel
{
    public const string ContentTypeName = "permission";

    public string DisplayName { get; }

    public bool IsEnabled { get; }

    public PermissionModel(
        [NotNull] string name,
        string displayName,
        bool isEnabled)
        : base(name)
    {
        DisplayName = displayName;
        IsEnabled = isEnabled;
    }
}
