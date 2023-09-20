using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionGrantInfo
{
    public string Name { get; }

    public bool IsGranted { get; }

    public string? ProviderName { get; }

    public string? ProviderKey { get; }

    public PermissionGrantInfo([NotNull] string name, bool isGranted, string? providerName = null, string? providerKey = null)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        IsGranted = isGranted;
        ProviderName = providerName;
        ProviderKey = providerKey;
    }
}
