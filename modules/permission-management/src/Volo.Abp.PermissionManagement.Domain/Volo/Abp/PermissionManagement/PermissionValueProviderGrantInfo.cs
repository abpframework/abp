using JetBrains.Annotations;

namespace Volo.Abp.PermissionManagement;

public class PermissionValueProviderGrantInfo //TODO: Rename to PermissionGrantInfo
{
    public static PermissionValueProviderGrantInfo NonGranted { get; } = new PermissionValueProviderGrantInfo(false);

    public virtual bool IsGranted { get; }

    public virtual string ProviderKey { get; }

    public PermissionValueProviderGrantInfo(bool isGranted, [CanBeNull] string providerKey = null)
    {
        IsGranted = isGranted;
        ProviderKey = providerKey;
    }
}
