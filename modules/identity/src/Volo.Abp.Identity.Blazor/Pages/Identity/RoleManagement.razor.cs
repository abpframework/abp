using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class RoleManagement
    {
        [Inject] private IIdentityRoleAppService RoleAppService { get; set; }
        [Inject] private IUiMessageService UiMessageService { get; set; }
        [Inject] private IObjectMapper<AbpIdentityBlazorModule> ObjectMapper { get; set; }

        private int _currentPage;
        private string _currentSorting;
        private int? _totalCount;

        private IReadOnlyList<IdentityRoleDto> _roles;

        private IdentityRoleCreateDto _newRole; //TODO: Would be better to create a UI model class
        private Guid _editingRoleId;
        private IdentityRoleUpdateDto _editingRole; //TODO: Would be better to create a UI model class

        private Modal _createModal;
        private Modal _editModal;

        public RoleManagement()
        {
            _newRole = new IdentityRoleCreateDto(); //TODO: Can we discard this (create on modal opening)?
            _editingRole = new IdentityRoleUpdateDto(); //TODO: Can we discard this (create on modal opening)?
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
            _editingRole = ObjectMapper.Map<IdentityRoleDto, IdentityRoleUpdateDto>(role);
            
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
