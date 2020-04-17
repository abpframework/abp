using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.PermissionManagement
{
    [RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("abp")]
    public class PermissionsController : AbpController, IPermissionAppService
    {
        protected IPermissionAppService PermissionAppService { get; }

        public PermissionsController(IPermissionAppService permissionAppService)
        {
            PermissionAppService = permissionAppService;
        }

        public virtual Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            return PermissionAppService.GetAsync(providerName, providerKey);
        }

        public virtual Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            return PermissionAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}
