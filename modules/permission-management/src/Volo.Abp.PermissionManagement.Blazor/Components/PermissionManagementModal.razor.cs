using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement.Blazor.Components
{
    public partial class PermissionManagementModal
    {
        [Inject] private IPermissionAppService PermissionAppService { get; set; }
        [Inject] private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

        private Modal _modal;

        private string _providerName;
        private string _providerKey;

        private string _entityDisplayName;
        private List<PermissionGroupDto> _groups;

        private List<PermissionGrantInfoDto> _disabledPermissions = new List<PermissionGrantInfoDto>();

        private string _selectedTabName;

        private int _grantedPermissionCount = 0;
        private int _notGrantedPermissionCount = 0;

        private bool GrantAll
        {
            get
            {
                if (_notGrantedPermissionCount == 0)
                {
                    return true;
                }

                return false;
            }
            set
            {
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

        public async Task OpenAsync(string providerName, string providerKey, string entityDisplayName = null)
        {
            try
            {
                _providerName = providerName;
                _providerKey = providerKey;

                var result = await PermissionAppService.GetAsync(_providerName, _providerKey);

                _entityDisplayName = entityDisplayName ?? result.EntityDisplayName;
                _groups = result.Groups;

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

        private Task CloseModal()
        {
            return InvokeAsync(_modal.Hide);
        }

        private async Task SaveAsync()
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

                await PermissionAppService.UpdateAsync(_providerName, _providerKey, updateDto);

                await CurrentApplicationConfigurationCacheResetService.ResetAsync();

                await InvokeAsync(_modal.Hide);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private string GetNormalizedGroupName(string name)
        {
            return "PermissionGroup_" + name.Replace(".", "_");
        }

        private void GroupGrantAllChanged(bool value, PermissionGroupDto permissionGroup)
        {
            foreach (var permission in permissionGroup.Permissions)
            {
                if (!IsDisabledPermission(permission))
                {
                    SetPermissionGrant(permission, value);
                }
            }
        }

        private void PermissionChanged(bool value, PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
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

        private PermissionGrantInfoDto GetParentPermission(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
        {
            return permissionGroup.Permissions.First(x => x.Name == permission.ParentName);
        }

        private List<PermissionGrantInfoDto> GetChildPermissions(PermissionGroupDto permissionGroup, PermissionGrantInfoDto permission)
        {
            return permissionGroup.Permissions.Where(x => x.Name.StartsWith(permission.Name)).ToList();
        }

        private bool IsDisabledPermission(PermissionGrantInfoDto permissionGrantInfo)
        {
            return _disabledPermissions.Any(x => x == permissionGrantInfo);
        }

        private string GetShownName(PermissionGrantInfoDto permissionGrantInfo)
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
    }
}
