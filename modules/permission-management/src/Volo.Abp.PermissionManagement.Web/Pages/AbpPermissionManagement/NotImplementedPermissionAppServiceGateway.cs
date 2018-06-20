using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement
{
    public class NotImplementedPermissionAppServiceGateway : IPermissionAppServiceGateway, ISingletonDependency
    {
        public Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            throw new System.NotImplementedException();
        }
    }
}