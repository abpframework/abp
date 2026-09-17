using JetBrains.Annotations;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionValueProviderGrantInfo  //TODO: Rename to ResourcePermissionGrantInfo
{
    public static ResourcePermissionValueProviderGrantInfo NonGranted { get; } = new ResourcePermissionValueProviderGrantInfo(false);

    public virtual bool IsGranted { get;  set; }

    public virtual string ProviderKey { get; set; }

    public ResourcePermissionValueProviderGrantInfo(bool isGranted, [CanBeNull] string providerKey = null)
    {
        IsGranted = isGranted;
        ProviderKey = providerKey;
    }
}
