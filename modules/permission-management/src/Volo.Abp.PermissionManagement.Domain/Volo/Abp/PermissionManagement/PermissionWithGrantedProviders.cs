using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.PermissionManagement;

public class PermissionWithGrantedProviders
{
    public string Name { get; }

    public bool IsGranted { get; set; }

    public List<PermissionValueProviderInfo> Providers { get; set; }

    public PermissionWithGrantedProviders([NotNull] string name, bool isGranted)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        IsGranted = isGranted;

        Providers = new List<PermissionValueProviderInfo>();
    }
}
