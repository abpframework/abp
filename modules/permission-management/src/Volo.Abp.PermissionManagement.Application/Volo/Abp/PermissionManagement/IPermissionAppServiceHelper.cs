using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.PermissionManagement
{
    public interface IPermissionAppServiceHelper
    {
        Task<GetPermissionListResultDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdatePermissionsDto input);
    }
}
