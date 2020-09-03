using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise.DataGrid;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace MyCompanyName.MyProjectName.Blazor.Pages
{
    //TODO: Idea: Can we create a base class to simplify all these, like CrudPage<...>..?

    public partial class RoleManagement
    {
        private int _currentPage;
        private int? _totalCount;
        private IReadOnlyList<IdentityRoleDto> _roles;
        private IdentityRoleCreateDto _newRole;

        public RoleManagement()
        {
            _newRole = new IdentityRoleCreateDto();
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
                    MaxResultCount = LimitedResultRequestDto.DefaultMaxResultCount
                });

            _roles = result.Items;
            _totalCount = (int?)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IdentityRoleDto> e)
        {
            _currentPage = e.Page - 1;
            await GetRolesAsync();
            StateHasChanged();
        }
        
        private async Task CreateRoleAsync()
        {
            await RoleAppService.CreateAsync(_newRole);
            _newRole = new IdentityRoleCreateDto();
            await GetRolesAsync();
        }

        private async Task DeleteRoleAsync(Guid id)
        {
            await RoleAppService.DeleteAsync(id);
            await GetRolesAsync();
        }
    }
}
