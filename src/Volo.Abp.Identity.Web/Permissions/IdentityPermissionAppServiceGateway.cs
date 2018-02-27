using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Permissions;
using Volo.Abp.Permissions.Web.Pages.AbpPermissions;

namespace Volo.Abp.Identity.Web.Permissions
{
    //TODO: Instead of creating such a gateway/adapter, we can implement a common interface for app services, like IHasPermissionManagementApi and manage it dynamically!

    public class IdentityPermissionAppServiceGateway : IPermissionAppServiceGateway, ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;
        private readonly IIdentityRoleAppService _roleAppService;

        public IdentityPermissionAppServiceGateway(IIdentityUserAppService userAppService, IIdentityRoleAppService roleAppService)
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;
        }

        public async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            switch (providerName)
            {
                case UserPermissionValueProvider.ProviderName:
                    return await _userAppService.GetPermissionsAsync(Guid.Parse(providerKey));
                case RolePermissionValueProvider.ProviderName:
                    return await _roleAppService.GetPermissionsAsync(Guid.Parse(providerKey));
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            switch (providerName)
            {
                case UserPermissionValueProvider.ProviderName:
                    await _userAppService.UpdatePermissionsAsync(Guid.Parse(providerKey), input);
                    break;
                case RolePermissionValueProvider.ProviderName:
                    await _roleAppService.UpdatePermissionsAsync(Guid.Parse(providerKey), input);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
