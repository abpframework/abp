using System.Collections.Generic;

namespace Volo.Abp.Authorization.Permissions.Resources;

public interface IResourcePermissionValueProviderManager
{
    IReadOnlyList<IResourcePermissionValueProvider> ValueProviders { get; }
}
