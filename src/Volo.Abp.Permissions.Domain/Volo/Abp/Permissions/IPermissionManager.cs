using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionManager
    {
        Task<PermissionWithGrantedProviders> GetAsync(string name, string providerName, string providerKey);

        Task<List<PermissionWithGrantedProviders>> GetAllAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task SetAsync(string name, string providerName, string providerKey, bool isGranted);
    }
}