using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models;

public abstract class PackageContentItemModel
{
    public string ContentType { get; }
    public string Name { get; }

    public PackageContentItemModel([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        ContentType = PackageContentItemNameAttribute.GetName(GetType());
    }
}
