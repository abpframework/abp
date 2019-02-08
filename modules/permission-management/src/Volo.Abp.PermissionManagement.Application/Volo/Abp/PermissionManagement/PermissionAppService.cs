using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement
{
    [Authorize]
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        protected PermissionManagementOptions Options { get; }

        private readonly IPermissionManager _permissionManager;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public PermissionAppService(
            IPermissionManager permissionManager, 
            IPermissionDefinitionManager permissionDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory,
            IOptions<PermissionManagementOptions> options)
        {
            Options = options.Value;
            _permissionManager = permissionManager;
            _permissionDefinitionManager = permissionDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            await CheckProviderPolicy(providerName);

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
                    if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
                    {
                        continue;
                    }

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

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }

        public async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            await CheckProviderPolicy(providerName);

            foreach (var permissionDto in input.Permissions)
            {
                var permissionDefinition = _permissionDefinitionManager.Get(permissionDto.Name);
                if (permissionDefinition.Providers.Any() && 
                    !permissionDefinition.Providers.Contains(providerName))
                {
                    throw new ApplicationException($"The permission named '{permissionDto.Name}' has not compatible with the provider named '{providerName}'");
                }

                await _permissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
            }
        }

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{policyName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
