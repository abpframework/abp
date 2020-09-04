using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace MyCompanyName.MyProjectName.Blazor.Pages
{
    public partial class RoleManagement
    {
        private int _currentPage;
        private string _currentSorting;
        private int? _totalCount;
        
        private IReadOnlyList<IdentityRoleDto> _roles;
        
        private IdentityRoleCreateDto _newRole;
        private Guid _editingRoleId;
        private IdentityRoleUpdateDto _editingRole;
        
        private Modal _createModal;
        private Modal _editModal;

        public RoleManagement()
        {
            _newRole = new IdentityRoleCreateDto();
            _editingRole = new IdentityRoleUpdateDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetRolesAsync();
        }

        private async Task GetRolesAsync()
        {
            var result = await RoleAppService.GetListAsync(
                new PagedAndSortedResultRequestDto
                {
                    SkipCount = _currentPage * LimitedResultRequestDto.DefaultMaxResultCount,
                    MaxResultCount = LimitedResultRequestDto.DefaultMaxResultCount,
                    Sorting = _currentSorting
                });

            _roles = result.Items;
            _totalCount = (int?)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IdentityRoleDto> e)
        {
            _currentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");

            _currentPage = e.Page - 1;
            await GetRolesAsync();
            StateHasChanged();
        }

        private void OpenCreateModal()
        {
            _newRole = new IdentityRoleCreateDto();
            _createModal.Show();
        }

        private void CloseCreateModal()
        {
            _createModal.Hide();
        }

        private async Task OpenEditModalAsync(Guid id)
        {
            var role = await RoleAppService.GetAsync(id);

            _editingRoleId = id;

            //TODO: User AutoMapper!
            _editingRole = new IdentityRoleUpdateDto
            {
                Name = role.Name,
                ConcurrencyStamp = role.ConcurrencyStamp,
                IsDefault = role.IsDefault,
                IsPublic = role.IsPublic
            };

            role.MapExtraPropertiesTo(_editingRole);

            _editModal.Show();
        }

        private void CloseEditModal()
        {
            _editModal.Hide();
        }

        private async Task CreateRoleAsync()
        {
            await RoleAppService.CreateAsync(_newRole);
            await GetRolesAsync();
            _createModal.Hide();
        }

        private async Task UpdateRoleAsync()
        {
            await RoleAppService.UpdateAsync(_editingRoleId, _editingRole);
            await GetRolesAsync();
            _editModal.Hide();
        }

        private async Task DeleteRoleAsync(IdentityRoleDto role)
        {
            if (!await UiMessageService.ConfirmAsync(L["RoleDeletionConfirmationMessage", role.Name]))
            {
                return;
            }

            await RoleAppService.DeleteAsync(role.Id);
            await GetRolesAsync();
        }
    }
}
