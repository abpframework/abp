using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using JetBrains.Annotations;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Authorization;
using Volo.Abp.BlazoriseUI.Components;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.BlazoriseUI
{
    public abstract class AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey>
        : AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey,
            PagedAndSortedResultRequestDto>
        where TAppService : ICrudAppService<
            TEntityDto,
            TKey>
        where TEntityDto : class, IEntityDto<TKey>, new()
    {
    }

    public abstract class AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey,
            TGetListInput>
        : AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey,
            TGetListInput,
            TEntityDto>
        where TAppService : ICrudAppService<
            TEntityDto,
            TKey,
            TGetListInput>
        where TEntityDto : class, IEntityDto<TKey>, new()
        where TGetListInput : new()
    {
    }

    public abstract class AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey,
            TGetListInput,
            TCreateInput>
        : AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TCreateInput>
        where TAppService : ICrudAppService<
            TEntityDto,
            TKey,
            TGetListInput,
            TCreateInput>
        where TEntityDto : IEntityDto<TKey>
        where TCreateInput : class, new()
        where TGetListInput : new()
    {
    }

    public abstract class AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        : AbpCrudPageBase<
            TAppService,
            TEntityDto,
            TEntityDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        where TAppService : ICrudAppService<
            TEntityDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        where TEntityDto : IEntityDto<TKey>
        where TCreateInput : class, new()
        where TUpdateInput : class, new()
        where TGetListInput : new()
    {
    }

    public abstract class AbpCrudPageBase<
            TAppService,
            TGetOutputDto,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        : AbpCrudPageBase<
            TAppService,
            TGetOutputDto,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput,
            TGetListOutputDto,
            TCreateInput,
            TUpdateInput>
        where TAppService : ICrudAppService<
            TGetOutputDto,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TCreateInput : class, new()
        where TUpdateInput : class, new()
        where TGetListInput : new()
    {
    }

    public abstract class AbpCrudPageBase<
            TAppService,
            TGetOutputDto,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput,
            TListViewModel,
            TCreateViewModel,
            TUpdateViewModel>
        : AbpComponentBase
        where TAppService : ICrudAppService<
            TGetOutputDto,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TCreateInput : class
        where TUpdateInput : class
        where TGetListInput : new()
        where TListViewModel : IEntityDto<TKey>
        where TCreateViewModel : class, new()
        where TUpdateViewModel : class, new()
    {
        [Inject] protected TAppService AppService { get; set; }
        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

        protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        protected int CurrentPage = 1;
        protected string CurrentSorting;
        protected int? TotalCount;
        protected TGetListInput GetListInput = new TGetListInput();
        protected IReadOnlyList<TListViewModel> Entities = Array.Empty<TListViewModel>();
        protected TCreateViewModel NewEntity;
        protected TKey EditingEntityId;
        protected TUpdateViewModel EditingEntity;
        protected Modal CreateModal;
        protected Modal EditModal;
        protected Validations CreateValidationsRef;
        protected Validations EditValidationsRef;
        protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
        protected DataGridEntityActionsColumn<TListViewModel> EntityActionsColumn;

        protected string CreatePolicyName { get; set; }
        protected string UpdatePolicyName { get; set; }
        protected string DeletePolicyName { get; set; }

        public bool HasCreatePermission { get; set; }
        public bool HasUpdatePermission { get; set; }
        public bool HasDeletePermission { get; set; }

        protected AbpCrudPageBase()
        {
            NewEntity = new TCreateViewModel();
            EditingEntity = new TUpdateViewModel();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual async Task SetPermissionsAsync()
        {
            if (CreatePolicyName != null)
            {
                HasCreatePermission = await AuthorizationService.IsGrantedAsync(CreatePolicyName);
            }

            if (UpdatePolicyName != null)
            {
                HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
            }

            if (DeletePolicyName != null)
            {
                HasDeletePermission = await AuthorizationService.IsGrantedAsync(DeletePolicyName);
            }
        }

        protected virtual async Task GetEntitiesAsync()
        {
            await UpdateGetListInputAsync();
            var result = await AppService.GetListAsync(GetListInput);
            Entities = MapToListViewModel(result.Items);
            TotalCount = (int?)result.TotalCount;
        }

        private IReadOnlyList<TListViewModel> MapToListViewModel(IReadOnlyList<TGetListOutputDto> dtos)
        {
            if (typeof(TGetListOutputDto) == typeof(TListViewModel))
            {
                return dtos.As<IReadOnlyList<TListViewModel>>();
            }

            return ObjectMapper.Map<IReadOnlyList<TGetListOutputDto>, List<TListViewModel>>(dtos);
        }

        protected virtual Task UpdateGetListInputAsync()
        {
            if (GetListInput is ISortedResultRequest sortedResultRequestInput)
            {
                sortedResultRequestInput.Sorting = CurrentSorting;
            }

            if (GetListInput is IPagedResultRequest pagedResultRequestInput)
            {
                pagedResultRequestInput.SkipCount = (CurrentPage - 1) * PageSize;
            }

            if (GetListInput is ILimitedResultRequest limitedResultRequestInput)
            {
                limitedResultRequestInput.MaxResultCount = PageSize;
            }

            return Task.CompletedTask;
        }

        protected virtual async Task SearchEntitiesAsync()
        {
            CurrentPage = 1;

            await GetEntitiesAsync();

            StateHasChanged();
        }

        protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TListViewModel> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;

            await GetEntitiesAsync();

            StateHasChanged();
        }

        protected virtual async Task OpenCreateModalAsync()
        {
            CreateValidationsRef?.ClearAll();

            await CheckCreatePolicyAsync();

            NewEntity = new TCreateViewModel();

            // Mapper will not notify Blazor that binded values are changed
            // so we need to notify it manually by calling StateHasChanged
            await InvokeAsync(() => StateHasChanged());

            CreateModal.Show();
        }

        protected virtual Task CloseCreateModalAsync()
        {
            CreateModal.Hide();
            return Task.CompletedTask;
        }

        protected virtual async Task OpenEditModalAsync(TListViewModel entity)
        {
            EditValidationsRef?.ClearAll();

            await CheckUpdatePolicyAsync();

            var entityDto = await AppService.GetAsync(entity.Id);

            EditingEntityId = entity.Id;
            EditingEntity = MapToEditingEntity(entityDto);

            await InvokeAsync(() => StateHasChanged());

            EditModal.Show();
        }

        protected virtual TUpdateViewModel MapToEditingEntity(TGetOutputDto entityDto)
        {
            return ObjectMapper.Map<TGetOutputDto, TUpdateViewModel>(entityDto);
        }

        protected virtual TCreateInput MapToCreateInput(TCreateViewModel createViewModel)
        {
            if (typeof(TCreateInput) == typeof(TCreateViewModel))
            {
                return createViewModel.As<TCreateInput>();
            }

            return ObjectMapper.Map<TCreateViewModel, TCreateInput>(createViewModel);
        }

        protected virtual TUpdateInput MapToUpdateInput(TUpdateViewModel updateViewModel)
        {
            if (typeof(TUpdateInput) == typeof(TUpdateViewModel))
            {
                return updateViewModel.As<TUpdateInput>();
            }

            return ObjectMapper.Map<TUpdateViewModel, TUpdateInput>(updateViewModel);
        }

        protected virtual Task CloseEditModalAsync()
        {
            EditModal.Hide();
            return Task.CompletedTask;
        }

        protected virtual async Task CreateEntityAsync()
        {
            if (CreateValidationsRef?.ValidateAll() ?? true)
            {
                await OnCreatingEntityAsync();

                await CheckCreatePolicyAsync();
                var createInput = MapToCreateInput(NewEntity);
                await AppService.CreateAsync(createInput);
                await GetEntitiesAsync();

                await OnCreatedEntityAsync();

                CreateModal.Hide();
            }
        }

        protected virtual Task OnCreatingEntityAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnCreatedEntityAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual async Task UpdateEntityAsync()
        {
            if (EditValidationsRef?.ValidateAll() ?? true)
            {
                await OnUpdatingEntityAsync();

                await CheckUpdatePolicyAsync();
                var updateInput = MapToUpdateInput(EditingEntity);
                await AppService.UpdateAsync(EditingEntityId, updateInput);
                await GetEntitiesAsync();

                await OnUpdatedEntityAsync();

                EditModal.Hide();
            }
        }

        protected virtual Task OnUpdatingEntityAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnUpdatedEntityAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual async Task DeleteEntityAsync(TListViewModel entity)
        {
            await CheckDeletePolicyAsync();

            await AppService.DeleteAsync(entity.Id);
            await GetEntitiesAsync();
        }

        protected virtual string GetDeleteConfirmationMessage(TListViewModel entity)
        {
            return UiLocalizer["ItemWillBeDeletedMessage"];
        }

        protected virtual async Task CheckCreatePolicyAsync()
        {
            await CheckPolicyAsync(CreatePolicyName);
        }

        protected virtual async Task CheckUpdatePolicyAsync()
        {
            await CheckPolicyAsync(UpdatePolicyName);
        }

        protected virtual async Task CheckDeletePolicyAsync()
        {
            await CheckPolicyAsync(DeletePolicyName);
        }

        /// <summary>
        /// Calls IAuthorizationService.CheckAsync for the given <see cref="policyName"/>.
        /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
        ///
        /// Does nothing if <see cref="policyName"/> is null or empty.
        /// </summary>
        /// <param name="policyName">A policy name to check</param>
        protected virtual async Task CheckPolicyAsync([CanBeNull] string policyName)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                return;
            }

            await AuthorizationService.CheckAsync(policyName);
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
