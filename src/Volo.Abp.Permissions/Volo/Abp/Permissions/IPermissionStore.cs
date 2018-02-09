using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionStore
    {
        Task<bool?> GetOrNullAsync([NotNull] string name, [CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task SetAsync([NotNull] string name, bool isGranted, [CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task<List<PermissionGrantInfo>> GetListAsync([CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task DeleteAsync([NotNull] string name, [CanBeNull]string providerName, [CanBeNull]string providerKey);
    }
}
