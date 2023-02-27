using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
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
    protected List<PermissionGroupDto> _groups;

    protected List<PermissionGrantInfoDto> _disabledPermissions = new List<PermissionGrantInfoDto>();

    protected string _selectedTabName;

    protected int _grantedPermissionCount = 0;
    protected int _notGrantedPermissionCount = 0;

    protected bool _selectAllDisabled;

    protected bool GrantAll {
        get {
            if (_notGrantedPermissionCount == 0)
            {
                return true;
            }

            return false;
        }
        set {
            if (_groups == null)
            {
                return;
            }

            _grantedPermissionCount = 0;
            _notGrantedPermissionCount = 0;

            foreach (var permission in _groups.SelectMany(x => x.Permissions))
            {
                if (!IsDisabledPermission(permission))
                {
                    permission.IsGranted = value;

                    if (value)
                    {
                        _grantedPermissionCount++;
                    }
                    else
                    {
                        _notGrantedPermissionCount++;
                    }
                }
            }
        }
    }

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

            var result = await PermissionAppService.GetAsync(_providerName, _providerKey);

            _entityDisplayName = entityDisplayName ?? result.EntityDisplayName;
            _groups = result.Groups;

            _selectAllDisabled = _groups.All(IsPermissionGroupDisabled);

            _grantedPermissionCount = 0;
            _notGrantedPermissionCount = 0;
            foreach (var permission in _groups.SelectMany(x => x.Permissions))
            {
                if (permission.IsGranted && permission.GrantedProviders.All(x => x.ProviderName != _providerName))
                {
                    _disabledPermissions.Add(permission);
                    continue;
                }

                if (permission.IsGranted)
                {
                    _grantedPermissionCount++;
                }
                else
                {
                    _notGrantedPermissionCount++;
                }
            }

            _selectedTabName = GetNormalizedGroupName(_groups.First().Name);

            await InvokeAsync(_modal.Show);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
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
                Permissions = _groups
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

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await InvokeAsync(_modal.Hide);
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

    protected virtual void GroupGrantAllChanged(bool value, PermissionGroupDto permissionGroup)
    {
        foreach (var permission in permissionGroup.Permissions)
        {
            if (!IsDisabledPermission(permission))
            {
                SetPermissionGrant(permission, value);
            }
        }
    }

    protected virtual void PermissionChanged(bool value, PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        SetPermissionGrant(permission, value);

        if (value && permission.ParentName != null)
        {
            var parentPermission = GetParentPermission(permissionGroup, permission);

            SetPermissionGrant(parentPermission, true);
        }
        else if (value == false)
        {
            var childPermissions = GetChildPermissions(permissionGroup, permission);

            foreach (var childPermission in childPermissions)
            {
                SetPermissionGrant(childPermission, false);
            }
        }
    }

    private void SetPermissionGrant(PermissionGrantInfoDto permission, bool value)
    {
        if (permission.IsGranted == value)
        {
            return;
        }

        if (value)
        {
            _grantedPermissionCount++;
            _notGrantedPermissionCount--;
        }
        else
        {
            _grantedPermissionCount--;
            _notGrantedPermissionCount++;
        }

        permission.IsGranted = value;
    }

    protected PermissionGrantInfoDto GetParentPermission(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        return permissionGroup.Permissions.First(x => x.Name == permission.ParentName);
    }

    protected List<PermissionGrantInfoDto> GetChildPermissions(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
    {
        return permissionGroup.Permissions.Where(x => x.Name.StartsWith(permission.Name)).ToList();
    }

    protected bool IsDisabledPermission(PermissionGrantInfoDto permissionGrantInfo)
    {
        return _disabledPermissions.Any(x => x == permissionGrantInfo);
    }

    protected virtual string GetShownName(PermissionGrantInfoDto permissionGrantInfo)
    {
        if (!IsDisabledPermission(permissionGrantInfo))
        {
            return permissionGrantInfo.DisplayName;
        }

        return string.Format(
            "{0} ({1})",
            permissionGrantInfo.DisplayName,
            permissionGrantInfo.GrantedProviders
                .Where(p => p.ProviderName != _providerName)
                .Select(p => p.ProviderName)
                .JoinAsString(", ")
        );
    }

    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual bool IsPermissionGroupDisabled(PermissionGroupDto group)
    {
        var permissions = group.Permissions;
        var grantedProviders = permissions.SelectMany(x => x.GrantedProviders);

        return permissions.All(x => x.IsGranted) && grantedProviders.All(p => p.ProviderName != _providerName);
    }
}
