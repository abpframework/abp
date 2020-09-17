using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.PermissionManagement.Blazor.Components
{
    public partial class PermissionManagementModal
    {
        [Inject] private IPermissionAppService PermissionAppService { get; set; }

        private Modal _modal;
        
        private string _providerName;
        private string _providerKey;

        private string _entityDisplayName;
        private List<PermissionGroupDto> _groups;

        private List<PermissionGrantInfoDto> _disabledPermissions;

        private string _selectedTabName;
        
        private bool GrantAll
        {
            get
            {
                return _groups != null && _groups.All(x => x.Permissions.All(y => y.IsGranted));
            }
            set
            {
                if (_groups == null)
                {
                    return;
                }
                
                foreach (var permissionGroupDto in _groups)
                {
                    foreach (var permission in permissionGroupDto.Permissions)
                    {
                        if (!IsDisabledPermission(permission))
                        {
                            permission.IsGranted = value;
                        }
                    }
                }
            }
        }
        
        public async Task OpenAsync(string providerName, string providerKey)
        {
            _providerName = providerName;
            _providerKey = providerKey;

            var result = await PermissionAppService.GetAsync(_providerName, _providerKey);

            _entityDisplayName = result.EntityDisplayName;
            _groups = result.Groups;

            _disabledPermissions = 
                _groups.SelectMany(x => x.Permissions)
                        .Where(
                            x => x.IsGranted &&
                            x.GrantedProviders.All(y => y.ProviderName != _providerName)
                            ).ToList();

            _selectedTabName = GetNormalizedGroupName(_groups.First().Name);
            
            _modal.Show();
        }

        private void CloseModal()
        {
            _modal.Hide();
        }

        private async Task SaveAsync()
        {
            var updateDto = new UpdatePermissionsDto
            {
                Permissions = _groups
                    .SelectMany(g => g.Permissions)
                    .Select(p => new UpdatePermissionDto {IsGranted = p.IsGranted, Name = p.Name})
                    .ToArray()
            };

            await PermissionAppService.UpdateAsync(_providerName, _providerKey, updateDto);

            _modal.Hide();
        }

        private string GetNormalizedGroupName(string name)
        {
            return "PermissionGroup_" + name.Replace(".", "_");
        }

        private void GrantAllChanged(bool value)
        {
            GrantAll = value;
        }

        private void GroupGrantAllChanged(bool value, string groupName)
        {
            foreach (var permission in _groups.First(x => x.Name == groupName).Permissions)
            {
                if (!IsDisabledPermission(permission))
                {
                    permission.IsGranted = value;
                }
            }
        }

        private void PermissionChanged(bool value, string groupName, PermissionGrantInfoDto permission)
        {
            permission.IsGranted = value;
            
            if (value == false)
            {
                var childPermissions = GetChildPermissions(groupName, permission);
                
                foreach (var permissionGrantInfoDto in childPermissions)
                {
                    permissionGrantInfoDto.IsGranted = false;
                }
            }
            else if (permission.ParentName != null)
            {
                var parentPermission = GetParentPermission(groupName, permission);

                parentPermission.IsGranted = true;
            }
        }

        private PermissionGrantInfoDto GetParentPermission(string groupName, PermissionGrantInfoDto permission)
        {
            return _groups.First(x => x.Name == groupName).Permissions.First(x => x.Name == permission.ParentName);
        }
        
        private List<PermissionGrantInfoDto> GetChildPermissions(string groupName, PermissionGrantInfoDto permission)
        {
            return _groups.First(x => x.Name == groupName).Permissions.Where(x => x.Name.StartsWith(permission.Name)).ToList();
        }
        
        private bool IsDisabledPermission(PermissionGrantInfoDto permissionGrantInfo)
        {
            return _disabledPermissions.Any(x => x == permissionGrantInfo);
        }
    }
}
