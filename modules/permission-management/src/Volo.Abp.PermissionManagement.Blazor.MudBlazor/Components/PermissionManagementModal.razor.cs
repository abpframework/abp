using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement.Blazor.MudBlazor.Components;

public partial class PermissionManagementModal
{
    [Inject] protected IPermissionAppService PermissionAppService { get; set; } = default!;
    [Inject] protected ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = default!;
    [Inject] protected IOptions<AbpLocalizationOptions> LocalizationOptions { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    protected bool _isVisible;

    protected string? _providerName;
    protected string? _providerKey;

    protected string? _entityDisplayName;
    protected List<PermissionGroupDto>? _allGroups;
    protected Dictionary<string, bool> _loadedPermissionValues = new Dictionary<string, bool>();
    protected List<PermissionGroupDto>? _groups;

    protected int _activeTabIndex = 0;

    protected bool _selectAllDisabled;

    protected string? _permissionGroupSearchText;

    protected bool GrantAll { get; set; }
    protected bool GrantAny { get; set; }

    protected Dictionary<string, int> _permissionDepths = new Dictionary<string, int>();

    public PermissionManagementModal()
    {
        LocalizationResource = typeof(AbpPermissionManagementResource);
    }

    public virtual async Task OpenAsync(string providerName, string providerKey, string? entityDisplayName = null)
    {
        try
        {
            _providerName = providerName;
            _providerKey = providerKey;
            _permissionGroupSearchText = null;

            var result = await PermissionAppService.GetAsync(_providerName, _providerKey);

            _entityDisplayName = entityDisplayName ?? result.EntityDisplayName;
            _allGroups = result.Groups.OrderBy(x => x.DisplayName).ToList();
            _groups = _allGroups.ToList();

            _loadedPermissionValues = _allGroups
                .SelectMany(x => x.Permissions)
                .ToDictionary(x => x.Name, x => x.IsGranted);

            NormalizePermissionGroup();

            GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
            GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);

            _isVisible = true;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task GrantAllAsync(bool grantAll)
    {
        GrantAll = grantAll;
        GrantAny = false;

        if (_allGroups == null)
        {
            return;
        }

        await ResetSearchTextAsync();

        foreach (var permission in _allGroups.SelectMany(x => x.Permissions))
        {
            if (IsDisabledPermission(permission))
            {
                continue;
            }

            permission.IsGranted = grantAll;
        }

        await InvokeAsync(StateHasChanged);
    }

    protected virtual void NormalizePermissionGroup(bool checkDisabledPermissions = true)
    {
        if (_groups == null)
        {
            return;
        }

        _selectAllDisabled = _groups.All(IsPermissionGroupDisabled);

        foreach (var group in _groups)
        {
            SetPermissionDepths(group.Permissions, null, 0);
        }

        if (_groups.Count != 0)
        {
            _activeTabIndex = 0;
        }
    }

    protected Task CloseModal()
    {
        _isVisible = false;
        return InvokeAsync(StateHasChanged);
    }

    protected virtual async Task SaveAsync()
    {
        try
        {
            if (_allGroups == null)
            {
                return;
            }

            var permissions = _allGroups.SelectMany(g => g.Permissions).ToList();

            var changedPermissions = permissions
                .Where(p => !_loadedPermissionValues.TryGetValue(p.Name, out var loadedValue) || loadedValue != p.IsGranted)
                .Select(p => new UpdatePermissionDto { IsGranted = p.IsGranted, Name = p.Name })
                .ToArray();

            if (!permissions.Any(p => p.IsGranted))
            {
                var confirmed = await DialogService.ShowMessageBoxAsync(
                    L["Warning"],
                    L["SaveWithoutAnyPermissionsWarningMessage"],
                    yesText: L["Yes"],
                    cancelText: L["Cancel"]);

                if (confirmed != true)
                {
                    return;
                }
            }

            if (changedPermissions.Any())
            {
                await PermissionAppService.UpdateAsync(_providerName!, _providerKey!, new UpdatePermissionsDto
                {
                    Permissions = changedPermissions
                });

                Guid? userId = null;
                if (_providerName == UserPermissionValueProvider.ProviderName && Guid.TryParse(_providerKey, out var parsedUserId))
                {
                    userId = parsedUserId;
                }

                await CurrentApplicationConfigurationCacheResetService.ResetAsync(userId);
            }

            _isVisible = false;
            await InvokeAsync(StateHasChanged);
            Snackbar.Add(L["SavedSuccessfully"], Severity.Success);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual string GetNormalizedGroupName(string name)
    {
        return "PermissionGroup_" + name.Replace(".", "_");
    }

    protected virtual void SetPermissionDepths(List<PermissionGrantInfoDto> permissions, string? currentParent, int currentDepth)
    {
        foreach (var item in permissions)
        {
            if (item.ParentName == currentParent)
            {
                _permissionDepths[item.Name] = currentDepth;
                SetPermissionDepths(permissions, item.Name, currentDepth + 1);
            }
        }
    }

    protected virtual int GetPermissionDepthOrDefault(string name)
    {
        return _permissionDepths.GetValueOrDefault(name, 0);
    }

    protected virtual async Task GroupGrantAllChanged(bool value, PermissionGroupDto permissionGroup)
    {
        foreach (var permission in permissionGroup.Permissions)
        {
            if (!IsDisabledPermission(permission))
            {
                SetPermissionGrant(permission, value);
            }
        }

        if (_allGroups != null)
        {
            GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
            GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);
        }
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task PermissionChanged(bool value, PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        SetPermissionGrant(permission, value);

        if (value)
        {
            SetParentPermissionGrant(permissionGroup, permission);
        }
        else
        {
            var childPermissions = GetChildPermissions(permissionGroup, permission);

            foreach (var childPermission in childPermissions)
            {
                SetPermissionGrant(childPermission, false);
            }
        }

        if (_allGroups != null)
        {
            GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
            GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);
        }
        await InvokeAsync(StateHasChanged);
    }

    private void SetParentPermissionGrant(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        if (permission.ParentName == null)
        {
            return;
        }

        var parentPermission = GetParentPermission(permissionGroup, permission);
        SetPermissionGrant(parentPermission, true);

        SetParentPermissionGrant(permissionGroup, parentPermission);
    }

    private void SetPermissionGrant(PermissionGrantInfoDto permission, bool value)
    {
        if (permission.IsGranted == value)
        {
            return;
        }

        permission.IsGranted = value;
    }

    protected virtual PermissionGrantInfoDto GetParentPermission(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        return permissionGroup.Permissions.First(x => x.Name == permission.ParentName);
    }

    protected virtual List<PermissionGrantInfoDto> GetChildPermissions(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        var childPermissions = new List<PermissionGrantInfoDto>();
        GetChildPermissions(childPermissions, permissionGroup.Permissions, permission);
        return childPermissions;
    }

    protected virtual void GetChildPermissions(List<PermissionGrantInfoDto> allChildPermissions, List<PermissionGrantInfoDto> permissions, PermissionGrantInfoDto permission)
    {
        var childPermissions = permissions.Where(x => x.ParentName == permission.Name).ToList();
        if (childPermissions.Count == 0)
        {
            return;
        }

        allChildPermissions.AddRange(childPermissions);

        foreach (var childPermission in childPermissions)
        {
            GetChildPermissions(allChildPermissions, permissions, childPermission);
        }
    }

    protected virtual bool IsDisabledPermission(PermissionGrantInfoDto permissionGrantInfo)
    {
        if (!permissionGrantInfo.IsEditable)
        {
            return true;
        }

        return permissionGrantInfo.IsGranted &&
               permissionGrantInfo.GrantedProviders.Any() &&
               permissionGrantInfo.GrantedProviders.All(p => p.ProviderName != _providerName);
    }

    protected virtual string GetShownName(PermissionGrantInfoDto permissionGrantInfo)
    {
        if (permissionGrantInfo.GrantedProviders.All(p => p.ProviderName == _providerName))
        {
            return permissionGrantInfo.DisplayName;
        }

        var grantedByOtherProviders = permissionGrantInfo.GrantedProviders
            .Where(p => p.ProviderName != _providerName)
            .ToList();

        if (!grantedByOtherProviders.Any())
        {
            return permissionGrantInfo.DisplayName;
        }

        return string.Format(
            "{0} ({1})",
            permissionGrantInfo.DisplayName,
            grantedByOtherProviders
                .Select(p => p.ProviderName)
                .JoinAsString(", ")
        );
    }

    protected virtual bool IsPermissionGroupDisabled(PermissionGroupDto group)
    {
        return group.Permissions.All(IsDisabledPermission);
    }

    protected virtual async Task ResetSearchTextAsync()
    {
        _permissionGroupSearchText = string.Empty;
        if (_allGroups != null)
        {
            _groups = _permissionGroupSearchText.IsNullOrWhiteSpace() 
                ? _allGroups.ToList() 
                : _allGroups.Where(x => x.DisplayName.Contains(_permissionGroupSearchText!, StringComparison.OrdinalIgnoreCase) || x.Permissions.Any(permission => permission.DisplayName.Contains(_permissionGroupSearchText!, StringComparison.OrdinalIgnoreCase))).ToList();

            NormalizePermissionGroup(false);
        }

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnPermissionGroupSearchTextChangedAsync(string? value)
    {
        if (value == _permissionGroupSearchText)
        {
            return;
        }

        _permissionGroupSearchText = value;
        if (_allGroups != null)
        {
            _groups = _permissionGroupSearchText.IsNullOrWhiteSpace() 
                ? _allGroups.ToList() 
                : _allGroups.Where(x => x.DisplayName.Contains(_permissionGroupSearchText!, StringComparison.OrdinalIgnoreCase) || x.Permissions.Any(permission => permission.DisplayName.Contains(_permissionGroupSearchText!, StringComparison.OrdinalIgnoreCase))).ToList();

            GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
            GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);

            NormalizePermissionGroup(false);
        }

        await InvokeAsync(StateHasChanged);
    }
}
