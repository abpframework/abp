using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement
{
    public interface IPermissionAppServiceGateway
    {
        Task<GetPermissionListResultDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdatePermissionsDto input);
    }
}
