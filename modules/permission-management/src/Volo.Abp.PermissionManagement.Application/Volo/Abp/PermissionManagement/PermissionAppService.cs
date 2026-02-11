using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement;

[Authorize]
public class PermissionAppService : ApplicationService, IPermissionAppService
{
    protected PermissionManagementOptions Options { get; }
    protected IPermissionManager PermissionManager { get; }
    protected IPermissionChecker PermissionChecker { get; }
    protected IResourcePermissionManager ResourcePermissionManager { get; }
    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

    public PermissionAppService(
        IPermissionManager permissionManager,
        IPermissionChecker permissionChecker,
        IPermissionDefinitionManager permissionDefinitionManager,
        IResourcePermissionManager resourcePermissionManager,
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager)
    {
        LocalizationResource = typeof(AbpPermissionManagementResource);
        ObjectMapperContext = typeof(AbpPermissionManagementApplicationModule);

        Options = options.Value;
        PermissionManager = permissionManager;
        PermissionChecker = permissionChecker;
        ResourcePermissionManager = resourcePermissionManager;
        ResourcePermissionGrantRepository = resourcePermissionGrantRepository;
        PermissionDefinitionManager = permissionDefinitionManager;
        SimpleStateCheckerManager = simpleStateCheckerManager;
    }

    public virtual async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
    {
        return await GetInternalAsync(null, providerName, providerKey);
    }

    public virtual async Task<GetPermissionListResultDto> GetByGroupAsync(string groupName, string providerName, string providerKey)
    {
        return await GetInternalAsync(groupName, providerName, providerKey);
    }

    protected virtual async Task<GetPermissionListResultDto> GetInternalAsync(string groupName, string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName);

        var result = new GetPermissionListResultDto
        {
            EntityDisplayName = providerKey,
            Groups = new List<PermissionGroupDto>()
        };

        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var permissionGroups = new List<PermissionGroupDto>();

        foreach (var group in (await PermissionDefinitionManager.GetGroupsAsync()).WhereIf(!groupName.IsNullOrWhiteSpace(), x => x.Name == groupName))
        {
            var groupDto = CreatePermissionGroupDto(group);
            var permissions = group.GetPermissionsWithChildren()
                .Where(x => x.IsEnabled)
                .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName))
                .Where(x => x.MultiTenancySide.HasFlag(multiTenancySide));

            var neededCheckPermissions = new List<PermissionDefinition>();
            foreach (var permission in permissions)
            {
                if (permission.Parent != null && !neededCheckPermissions.Contains(permission.Parent))
                {
                    continue;
                }

                if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                {
                    neededCheckPermissions.Add(permission);
                }
            }

            if (!neededCheckPermissions.Any())
            {
                continue;
            }

            groupDto.Permissions.AddRange(neededCheckPermissions.Select(CreatePermissionGrantInfoDto));
            permissionGroups.Add(groupDto);
        }

        var multipleGrantInfo = await PermissionManager.GetAsync(
            permissionGroups.SelectMany(group => group.Permissions).Select(permission => permission.Name).ToArray(),
            providerName,
            providerKey);

        foreach (var permissionGroup in permissionGroups)
        {
            foreach (var permission in permissionGroup.Permissions)
            {
                var grantInfo = multipleGrantInfo.Result.FirstOrDefault(x => x.Name == permission.Name);
                if (grantInfo == null)
                {
                    continue;
                }

                permission.IsGranted = grantInfo.IsGranted;
                permission.GrantedProviders = grantInfo.Providers.Select(x => new ProviderInfoDto
                {
                    ProviderName = x.Name,
                    ProviderKey = x.Key,
                }).ToList();
            }

            if (permissionGroup.Permissions.Any())
            {
                result.Groups.Add(permissionGroup);
            }
        }

        return result;
    }

    protected virtual PermissionGrantInfoDto CreatePermissionGrantInfoDto(PermissionDefinition permission)
    {
        return new PermissionGrantInfoDto
        {
            Name = permission.Name,
            DisplayName = permission.DisplayName?.Localize(StringLocalizerFactory),
            ParentName = permission.Parent?.Name,
            AllowedProviders = permission.Providers,
            GrantedProviders = new List<ProviderInfoDto>()
        };
    }

    protected virtual PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
    {
        var localizableDisplayName = group.DisplayName as LocalizableString;

        return new PermissionGroupDto
        {
            Name = group.Name,
            DisplayName = group.DisplayName?.Localize(StringLocalizerFactory),
            DisplayNameKey = localizableDisplayName?.Name,
            DisplayNameResource = localizableDisplayName?.ResourceType != null
                ? LocalizationResourceNameAttribute.GetName(localizableDisplayName.ResourceType)
                : null,
            Permissions = new List<PermissionGrantInfoDto>()
        };
    }

    public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        await CheckProviderPolicy(providerName);

        foreach (var permissionDto in input.Permissions)
        {
            await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
        }
    }

    public virtual async Task<GetResourceProviderListResultDto> GetResourceProviderKeyLookupServicesAsync(string resourceName)
    {
        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        if (!resourcePermissions.Any() ||
            !await AuthorizationService.IsGrantedAnyAsync(resourcePermissions.Select(p => p.ManagementPermissionName!).ToArray()))
        {
            return new GetResourceProviderListResultDto
            {
                Providers = new List<ResourceProviderDto>()
            };
        }

        var lookupServices = await ResourcePermissionManager.GetProviderKeyLookupServicesAsync();
        return new GetResourceProviderListResultDto
        {
            Providers = lookupServices.Select(s => new ResourceProviderDto
            {
                Name = s.Name,
                DisplayName = s.DisplayName.Localize(StringLocalizerFactory),
            }).ToList()
        };
    }

    public virtual async Task<SearchProviderKeyListResultDto> SearchResourceProviderKeyAsync(string resourceName, string serviceName, string filter, int page)
    {
        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        if (resourcePermissions.IsNullOrEmpty() ||
            !await AuthorizationService.IsGrantedAnyAsync(resourcePermissions.Select(p => p.ManagementPermissionName!).ToArray()))
        {
            return new SearchProviderKeyListResultDto();
        }

        var lookupService = await ResourcePermissionManager.GetProviderKeyLookupServiceAsync(serviceName);
        var keys = await lookupService.SearchAsync(filter, page);
        return new SearchProviderKeyListResultDto
        {
            Keys = keys.Select(x => new SearchProviderKeyInfo
            {
                ProviderKey =  x.ProviderKey,
                ProviderDisplayName =  x.ProviderDisplayName,
            }).ToList()
        };
    }

    public virtual async Task<GetResourcePermissionDefinitionListResultDto> GetResourceDefinitionsAsync(string resourceName)
    {
        var result = new GetResourcePermissionDefinitionListResultDto
        {
            Permissions = new List<ResourcePermissionDefinitionDto>()
        };

        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        var permissionGrants = (await PermissionChecker.IsGrantedAsync(resourcePermissions
                .Select(rp => rp.ManagementPermissionName!)
                .Distinct().ToArray())).Result.Where(x => x.Value == PermissionGrantResult.Granted).Select(x => x.Key)
            .ToHashSet();
        foreach (var resourcePermission in resourcePermissions)
        {
            if (!permissionGrants.Contains(resourcePermission.ManagementPermissionName))
            {
                continue;
            }

            result.Permissions.Add(new ResourcePermissionDefinitionDto
            {
                Name = resourcePermission.Name,
                DisplayName = resourcePermission.DisplayName?.Localize(StringLocalizerFactory),
            });
        }

        return result;
    }

    public virtual async Task<GetResourcePermissionListResultDto> GetResourceAsync(string resourceName, string resourceKey)
    {
        var result = new GetResourcePermissionListResultDto
        {
            Permissions = new List<ResourcePermissionGrantInfoDto>()
        };

        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        var resourcePermissionGrants = await ResourcePermissionManager.GetAllGroupAsync(resourceName, resourceKey);
        var permissionGrants = (await PermissionChecker.IsGrantedAsync(resourcePermissions
                .Select(rp => rp.ManagementPermissionName!)
                .Distinct().ToArray())).Result.Where(x => x.Value == PermissionGrantResult.Granted).Select(x => x.Key)
            .ToHashSet();
        foreach (var resourcePermissionGrant in resourcePermissionGrants)
        {
            var resourcePermissionGrantInfoDto = new ResourcePermissionGrantInfoDto
            {
                ProviderName = resourcePermissionGrant.ProviderName,
                ProviderKey = resourcePermissionGrant.ProviderKey,
                ProviderDisplayName = resourcePermissionGrant.ProviderDisplayName,
                ProviderNameDisplayName  = resourcePermissionGrant.ProviderNameDisplayName?.Localize(StringLocalizerFactory),
                Permissions = new List<GrantedResourcePermissionDto>()
            };
            foreach (var permission in resourcePermissionGrant.Permissions)
            {
                var resourcePermission = resourcePermissions.FirstOrDefault(x => x.Name == permission);
                if (resourcePermission == null)
                {
                    continue;
                }

                if (!permissionGrants.Contains(resourcePermission.ManagementPermissionName))
                {
                    continue;
                }

                resourcePermissionGrantInfoDto.Permissions.Add(new GrantedResourcePermissionDto()
                {
                    Name = permission,
                    DisplayName = resourcePermission?.DisplayName.Localize(StringLocalizerFactory),
                });
            }

            if(resourcePermissionGrantInfoDto.Permissions.Any())
            {
                result.Permissions.Add(resourcePermissionGrantInfoDto);
            }
        }

        return result;
    }

    public virtual async Task<GetResourcePermissionWithProviderListResultDto> GetResourceByProviderAsync(string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var result = new GetResourcePermissionWithProviderListResultDto
        {
            Permissions = new List<ResourcePermissionWithProdiverGrantInfoDto>()
        };

        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        var resourcePermissionGrants = await ResourcePermissionManager.GetAllAsync(resourceName, resourceKey, providerName, providerKey);
        var permissionGrants = (await PermissionChecker.IsGrantedAsync(resourcePermissions
                .Select(rp => rp.ManagementPermissionName!)
                .Distinct().ToArray())).Result.Where(x => x.Value == PermissionGrantResult.Granted).Select(x => x.Key)
            .ToHashSet();
        foreach (var resourcePermissionGrant in resourcePermissionGrants)
        {
            var resourcePermission = resourcePermissions.FirstOrDefault(x => x.Name == resourcePermissionGrant.Name);
            if (resourcePermission == null)
            {
                continue;
            }

            if (!permissionGrants.Contains(resourcePermission.ManagementPermissionName))
            {
                continue;
            }

            result.Permissions.Add(new ResourcePermissionWithProdiverGrantInfoDto
            {
                Name = resourcePermissionGrant.Name,
                DisplayName = resourcePermission?.DisplayName.Localize(StringLocalizerFactory),
                Providers = resourcePermissionGrant.Providers.Select(x => x.Name).ToList(),
                IsGranted = resourcePermissionGrant.IsGranted
            });
        }

        return result;
    }

    public virtual async Task UpdateResourceAsync(string resourceName, string resourceKey, UpdateResourcePermissionsDto input)
    {
        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        var permissionGrants = (await PermissionChecker.IsGrantedAsync(resourcePermissions
                .Select(rp => rp.ManagementPermissionName!)
                .Distinct().ToArray())).Result.Where(x => x.Value == PermissionGrantResult.Granted).Select(x => x.Key)
            .ToHashSet();
        foreach (var resourcePermission in resourcePermissions)
        {
            if (!permissionGrants.Contains(resourcePermission.ManagementPermissionName))
            {
                continue;
            }

            var isGranted = !input.Permissions.IsNullOrEmpty() && input.Permissions.Any(p => p == resourcePermission.Name);
            await ResourcePermissionManager.SetAsync(resourcePermission.Name, resourceName, resourceKey, input.ProviderName, input.ProviderKey, isGranted);
        }
    }

    public virtual async Task DeleteResourceAsync(string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var resourcePermissions = await ResourcePermissionManager.GetAvailablePermissionsAsync(resourceName);
        var permissionGrants = (await PermissionChecker.IsGrantedAsync(resourcePermissions
                .Select(rp => rp.ManagementPermissionName!)
                .Distinct().ToArray())).Result.Where(x => x.Value == PermissionGrantResult.Granted).Select(x => x.Key)
            .ToHashSet();
        foreach (var resourcePermission in resourcePermissions)
        {
            if (!permissionGrants.Contains(resourcePermission.ManagementPermissionName))
            {
                continue;
            }

            await ResourcePermissionManager.DeleteAsync(resourcePermission.Name, resourceName, resourceKey, providerName, providerKey);
        }
    }

    protected virtual async Task CheckProviderPolicy(string providerName)
    {
        var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
        if (policyName.IsNullOrEmpty())
        {
            throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
        }

        await AuthorizationService.CheckAsync(policyName);
    }
}
