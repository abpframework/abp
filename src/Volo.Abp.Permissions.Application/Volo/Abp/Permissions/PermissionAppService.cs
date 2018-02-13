using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Permissions
{
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionAppService(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task<PermissionGrantInfoDto> GetAsync(string name, string providerName, string providerKey)
        {
            var result = await _permissionManager.IsGrantedAsync(name, providerName, providerKey);

            throw new NotImplementedException();
        }

        public async Task<List<PermissionGrantInfoDto>> GetListAsync(string providerName, string providerKey)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            throw new NotImplementedException();
        }
    }
}
