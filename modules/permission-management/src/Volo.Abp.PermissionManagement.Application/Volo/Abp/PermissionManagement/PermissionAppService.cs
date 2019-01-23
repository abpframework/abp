using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement
{
    //[Authorize]
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public PermissionAppService(
            IPermissionManager permissionManager, 
            IPermissionDefinitionManager permissionDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _permissionManager = permissionManager;
            _permissionDefinitionManager = permissionDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            var result = new GetPermissionListResultDto
            {
                EntityDisplayName = providerKey,
                Groups = new List<PermissionGroupDto>()
            };

            foreach (var group in _permissionDefinitionManager.GetGroups())
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName.Localize(_stringLocalizerFactory),
                    Permissions = new List<PermissionGrantInfoDto>()
                };

                foreach (var permission in group.GetPermissionsWithChildren())
                {
                    var grantInfoDto = new PermissionGrantInfoDto
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName.Localize(_stringLocalizerFactory),
                        ParentName = permission.Parent?.Name,
                        Providers = new List<ProviderInfoDto>()
                    };

                    var grantInfo = await _permissionManager.GetAsync(permission.Name, providerName, providerKey);

                    grantInfoDto.IsGranted = grantInfo.IsGranted;

                    foreach (var provider in grantInfo.Providers)
                    {
                        grantInfoDto.Providers.Add(new ProviderInfoDto
                        {
                            ProviderName = provider.Name,
                            ProviderKey = provider.Key,
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                result.Groups.Add(groupDto);
            }

            return result;
        }

        public async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            foreach (var permission in input.Permissions)
            {
                await _permissionManager.SetAsync(permission.Name, providerName, providerKey, permission.IsGranted);
            }
        }
    }
}
