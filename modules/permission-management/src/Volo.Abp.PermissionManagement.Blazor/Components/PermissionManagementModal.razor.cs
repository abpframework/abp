using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement.Blazor.Components;

public partial class PermissionManagementModal
{
    [Inject] protected IPermissionAppService PermissionAppService { get; set; }
    [Inject] protected ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

    [Inject] protected IOptions<AbpLocalizationOptions> LocalizationOptions { get; set; }

    protected Modal _modal;

    protected string _providerName;
    protected string _providerKey;

    protected string _entityDisplayName;
    protected List<PermissionGroupDto> _allGroups;
    protected List<PermissionGroupDto> _groups;

    protected string _selectedTabName;

    protected bool _selectAllDisabled;

    protected string _permissionGroupSearchText;

    protected bool GrantAll { get; set; }
    protected bool GrantAny { get; set; }

    protected Dictionary<string, int> _permissionDepths = new Dictionary<string, int>();

    public PermissionManagementModal()
    {
        LocalizationResource = typeof(AbpPermissionManagementResource);
    }

    public virtual async Task OpenAsync(string providerName, string providerKey, string entityDisplayName = null)
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

            NormalizePermissionGroup();

            GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
            GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);

            await InvokeAsync(_modal.Show);
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
        _selectAllDisabled = _groups.All(IsPermissionGroupDisabled);

        foreach (var group in _groups)
        {
            SetPermissionDepths(group.Permissions, null, 0);
        }

        if (_groups.Count != 0)
        {
            _selectedTabName = GetNormalizedGroupName(_groups.First().Name);
        }
    }

    protected Task CloseModal()
    {
        return InvokeAsync(_modal.Hide);
    }

    protected virtual async Task SaveAsync()
    {
        try
        {

            var updateDto = new UpdatePermissionsDto
            {
                Permissions = _allGroups
                    .SelectMany(g => g.Permissions)
                    .Select(p => new UpdatePermissionDto { IsGranted = p.IsGranted, Name = p.Name })
                    .ToArray()
            };

            if (!updateDto.Permissions.Any(x => x.IsGranted))
            {
                if (!await Message.Confirm(L["SaveWithoutAnyPermissionsWarningMessage"].Value))
                {
                    return;
                }
            }

            await PermissionAppService.UpdateAsync(_providerName, _providerKey, updateDto);

            Guid? userId = null;
            if (_providerName == UserPermissionValueProvider.ProviderName && Guid.TryParse(_providerKey, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            await CurrentApplicationConfigurationCacheResetService.ResetAsync(userId);

            await InvokeAsync(_modal.Hide);
            await Notify.Success(L["SavedSuccessfully"]);
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

    protected virtual void SetPermissionDepths(List<PermissionGrantInfoDto> permissions, string currentParent, int currentDepth)
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

        GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
        GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);
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

        GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
        GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);
        await InvokeAsync(StateHasChanged);
    }

    private void SetParentPermissionGrant(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        if(permission.ParentName == null)
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
            .Select(p => p.ProviderName)
            .ToList();

        if (!grantedByOtherProviders.Any())
        {
            return permissionGrantInfo.DisplayName;
        }

        return string.Format(
            "{0} ({1})",
            permissionGrantInfo.DisplayName,
            grantedByOtherProviders.JoinAsString(", ")
        );
    }

    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual bool IsPermissionGroupDisabled(PermissionGroupDto group)
    {
        return group.Permissions.All(IsDisabledPermission);
    }

    protected virtual async Task ResetSearchTextAsync()
    {
        _permissionGroupSearchText = string.Empty;
        _groups = _permissionGroupSearchText.IsNullOrWhiteSpace() ? _allGroups.ToList() : _allGroups.Where(x => x.DisplayName.Contains(_permissionGroupSearchText, StringComparison.OrdinalIgnoreCase) || x.Permissions.Any(permission => permission.DisplayName.Contains(_permissionGroupSearchText, StringComparison.OrdinalIgnoreCase))).ToList();

        NormalizePermissionGroup(false);

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnPermissionGroupSearchTextChangedAsync(string value)
    {
        if (value == _permissionGroupSearchText)
        {
            return;
        }

        _permissionGroupSearchText = value;
        _groups = _permissionGroupSearchText.IsNullOrWhiteSpace() ? _allGroups.ToList() : _allGroups.Where(x => x.DisplayName.Contains(_permissionGroupSearchText, StringComparison.OrdinalIgnoreCase) || x.Permissions.Any(permission => permission.DisplayName.Contains(_permissionGroupSearchText, StringComparison.OrdinalIgnoreCase))).ToList();

        GrantAll = _allGroups.SelectMany(x => x.Permissions).All(p => p.IsGranted);
        GrantAny = !GrantAll && _allGroups.SelectMany(x => x.Permissions).Any(p => p.IsGranted);

        NormalizePermissionGroup(false);

        await InvokeAsync(StateHasChanged);
    }

    protected virtual Task OnSelectedTabChangedAsync(string name)
    {
        _selectedTabName = name;

        return Task.CompletedTask;
    }
}
