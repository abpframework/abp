using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.PermissionManagement
{
    [RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("permissionManagement")]
    [Route("api/permission-management/permissions")]
    public class PermissionsController : AbpControllerBase, IPermissionAppService
    {
        protected IPermissionAppService PermissionAppService { get; }

        public PermissionsController(IPermissionAppService permissionAppService)
        {
            PermissionAppService = permissionAppService;
        }

        [HttpGet]
        public virtual Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            return PermissionAppService.GetAsync(providerName, providerKey);
        }

        [HttpPut]
        public virtual Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            return PermissionAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}
