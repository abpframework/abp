using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.PermissionManagement
{
    [RemoteService]
    [Area("abp")]
    public class PermissionsController : AbpController, IPermissionAppService
    {
        private readonly IPermissionAppService _permissionAppService;

        public PermissionsController(IPermissionAppService permissionAppService)
        {
            _permissionAppService = permissionAppService;
        }

        public Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            return _permissionAppService.GetAsync(providerName, providerKey);
        }

        public Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            return _permissionAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}
