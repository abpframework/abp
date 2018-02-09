using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    //TODO: Change fallback to inherit?

    public interface IPermissionManager
    {
        Task<bool> IsGrantedAsync([NotNull]string name);

        Task<bool> IsGrantedAsync([NotNull]string name, [NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

        Task<List<PermissionGrantInfo>> GetAllAsync();

        Task<List<PermissionGrantInfo>> GetAllAsync([NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

        Task SetAsync([NotNull] string name, bool? isGranted, [NotNull] string providerName, [CanBeNull] string providerKey, bool forceToSet = false);
    }
}