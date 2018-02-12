using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync([NotNull] string name, [CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task<List<string>> GetAllGrantedAsync([CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task AddAsync([NotNull] string name, [CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task RemoveAsync([NotNull] string name, [CanBeNull]string providerName, [CanBeNull]string providerKey);
    }
}
