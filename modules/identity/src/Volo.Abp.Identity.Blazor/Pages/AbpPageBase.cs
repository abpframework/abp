using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Identity.Blazor.Pages
{
    public abstract class AbpPageBase<
        TAppService,
        TEntityDto,
        TKey,
        TGetListInput,
        TCreateInput,
        TUpdateInput>
        : ComponentBase
        where TAppService : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TKey>
        where TCreateInput : new()
        where TUpdateInput : new()
        where TGetListInput : PagedAndSortedResultRequestDto, new()
    {
        [Inject] protected TAppService AppService { get; set; }
        [Inject] protected IObjectMapper<AbpIdentityBlazorModule> ObjectMapper { get; set; }

        protected int _currentPage;
        protected string _currentSorting;
        protected int? _totalCount;

        protected IReadOnlyList<TEntityDto> _roles;

        protected TCreateInput _newRole;
        protected TKey _editingRoleId;
        protected TUpdateInput _editingRole;

        protected Modal _createModal;
        protected Modal _editModal;

        protected AbpPageBase()
        {
            _newRole = new TCreateInput();
            _editingRole = new TUpdateInput();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetRolesAsync();
        }

        protected async Task GetRolesAsync()
        {
            var result = await AppService.GetListAsync(
                new TGetListInput
                {
                    SkipCount = _currentPage * LimitedResultRequestDto.DefaultMaxResultCount,
                    MaxResultCount = LimitedResultRequestDto.DefaultMaxResultCount,
                    Sorting = _currentSorting
                });

            _roles = result.Items;
            _totalCount = (int?)result.TotalCount;
        }

        protected async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TEntityDto> e)
        {
            _currentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");

            _currentPage = e.Page - 1;
            await GetRolesAsync();
            StateHasChanged();
        }

        protected void OpenCreateModal()
        {
            _newRole = new TCreateInput();
            _createModal.Show();
        }

        protected void CloseCreateModal()
        {
            _createModal.Hide();
        }

        protected async Task OpenEditModalAsync(TKey id)
        {
            var role = await AppService.GetAsync(id);

            _editingRoleId = id;
            _editingRole = ObjectMapper.Map<TEntityDto, TUpdateInput>(role);

            _editModal.Show();
        }

        protected void CloseEditModal()
        {
            _editModal.Hide();
        }

        protected async Task CreateRoleAsync()
        {
            await AppService.CreateAsync(_newRole);
            await GetRolesAsync();
            _createModal.Hide();
        }

        protected async Task UpdateRoleAsync()
        {
            await AppService.UpdateAsync(_editingRoleId, _editingRole);
            await GetRolesAsync();
            _editModal.Hide();
        }

        protected async Task DeleteRoleAsync(TEntityDto role)
        {
            //if (!await UiMessageService.ConfirmAsync(L["RoleDeletionConfirmationMessage", role.Name]))
            //{
            //    return;
            //}

            await AppService.DeleteAsync(role.Id);
            await GetRolesAsync();
        }
    }
}