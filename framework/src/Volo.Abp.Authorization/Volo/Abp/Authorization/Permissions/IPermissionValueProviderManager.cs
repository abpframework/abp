using System.Collections.Generic;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionValueProviderManager
    {
        IReadOnlyList<IPermissionValueProvider> ValueProviders { get; }
    }
}