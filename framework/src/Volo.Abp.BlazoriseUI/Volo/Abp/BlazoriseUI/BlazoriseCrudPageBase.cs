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
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Authorization;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.BlazoriseUI
{
    public abstract class BlazoriseCrudPageBase<TAppService, TEntityDto, TKey>
        : BlazoriseCrudPageBase<TAppService, TEntityDto, TKey, PagedAndSortedResultRequestDto>
        where TAppService : ICrudAppService<TEntityDto, TKey>
        where TEntityDto : IEntityDto<TKey>, new()
    {
    }

    public abstract class BlazoriseCrudPageBase<TAppService, TEntityDto, TKey, TGetListInput>
        : BlazoriseCrudPageBase<TAppService, TEntityDto, TKey, TGetListInput, TEntityDto>
        where TAppService : ICrudAppService<TEntityDto, TKey, TGetListInput>
        where TEntityDto : IEntityDto<TKey>, new()
        where TGetListInput : new()
    {
    }

    public abstract class BlazoriseCrudPageBase<TAppService, TEntityDto, TKey, TGetListInput, TCreateInput>
        : BlazoriseCrudPageBase<TAppService, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TAppService : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput>
        where TEntityDto : IEntityDto<TKey>
        where TCreateInput : new()
        where TGetListInput : new()
    {
    }

    public abstract class BlazoriseCrudPageBase<TAppService, TEntityDto, TKey, TGetListInput, TCreateInput,
            TUpdateInput>
        : BlazoriseCrudPageBase<TAppService, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TAppService : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TKey>
        where TCreateInput : new()
        where TUpdateInput : new()
        where TGetListInput : new()
    {
    }

    public abstract class BlazoriseCrudPageBase<TAppService, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput,
            TCreateInput, TUpdateInput>
        : OwningComponentBase
        where TAppService : ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput,
            TUpdateInput>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TCreateInput : new()
        where TUpdateInput : new()
        where TGetListInput : new()
    {
        [Inject] protected TAppService AppService { get; set; }
        [Inject] protected IUiMessageService UiMessageService { get; set; }
        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }
        [Inject] protected IAuthorizationService AuthorizationService { get; set; }

        protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        protected int CurrentPage;
        protected string CurrentSorting;
        protected int? TotalCount;
        protected IReadOnlyList<TGetListOutputDto> Entities = Array.Empty<TGetListOutputDto>();
        protected TCreateInput NewEntity;
        protected TKey EditingEntityId;
        protected TUpdateInput EditingEntity;
        protected Modal CreateModal;
        protected Modal EditModal;

        protected string CreatePolicyName { get; set; }
        protected string UpdatePolicyName { get; set; }
        protected string DeletePolicyName { get; set; }

        public bool HasCreatePermission { get; set; }
        public bool HasUpdatePermission { get; set; }
        public bool HasDeletePermission { get; set; }

        protected Type ObjectMapperContext { get; set; }

        protected IObjectMapper ObjectMapper
        {
            get
            {
                if (_objectMapper != null)
                {
                    return _objectMapper;
                }

                if (ObjectMapperContext == null)
                {
                    return LazyGetRequiredService(ref _objectMapper);
                }

                return LazyGetRequiredService(
                    typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext),
                    ref _objectMapper
                );
            }
        }

        private IObjectMapper _objectMapper;

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                reference = (TRef) ScopedServices.GetRequiredService(serviceType);
            }

            return reference;
        }

        protected BlazoriseCrudPageBase()
        {
            NewEntity = new TCreateInput();
            EditingEntity = new TUpdateInput();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetEntitiesAsync();
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
            var input = await CreateGetListInputAsync();
            var result = await AppService.GetListAsync(input);
            Entities = result.Items;
            TotalCount = (int?) result.TotalCount;
        }

        protected virtual Task<TGetListInput> CreateGetListInputAsync()
        {
            var input = new TGetListInput();

            if (input is ISortedResultRequest sortedResultRequestInput)
            {
                sortedResultRequestInput.Sorting = CurrentSorting;
            }

            if (input is IPagedResultRequest pagedResultRequestInput)
            {
                pagedResultRequestInput.SkipCount = CurrentPage * PageSize;
            }

            if (input is ILimitedResultRequest limitedResultRequestInput)
            {
                limitedResultRequestInput.MaxResultCount = PageSize;
            }

            return Task.FromResult(input);
        }

        protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TGetListOutputDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetEntitiesAsync();

            StateHasChanged();
        }

        protected virtual async Task OpenCreateModalAsync()
        {
            await CheckCreatePolicyAsync();

            NewEntity = new TCreateInput();
            CreateModal.Show();
        }

        protected virtual Task CloseCreateModalAsync()
        {
            CreateModal.Hide();
            return Task.CompletedTask;
        }

        protected virtual async Task OpenEditModalAsync(TKey id)
        {
            await CheckUpdatePolicyAsync();

            var entityDto = await AppService.GetAsync(id);
            EditingEntityId = id;
            EditingEntity = MapToEditingEntity(entityDto);
            EditModal.Show();
        }

        protected virtual TUpdateInput MapToEditingEntity(TGetOutputDto entityDto)
        {
            return ObjectMapper.Map<TGetOutputDto, TUpdateInput>(entityDto);
        }

        protected virtual Task CloseEditModalAsync()
        {
            EditModal.Hide();
            return Task.CompletedTask;
        }

        protected virtual async Task CreateEntityAsync()
        {
            await CheckCreatePolicyAsync();
            await AppService.CreateAsync(NewEntity);
            await GetEntitiesAsync();
            CreateModal.Hide();
        }

        protected virtual async Task UpdateEntityAsync()
        {
            await CheckUpdatePolicyAsync();
            await AppService.UpdateAsync(EditingEntityId, EditingEntity);
            await GetEntitiesAsync();
            EditModal.Hide();
        }

        protected virtual async Task DeleteEntityAsync(TGetListOutputDto entity)
        {
            await CheckDeletePolicyAsync();

            if (!await UiMessageService.ConfirmAsync(GetDeleteConfirmationMessage(entity)))
            {
                return;
            }

            await AppService.DeleteAsync(entity.Id);
            await GetEntitiesAsync();
        }

        protected virtual string GetDeleteConfirmationMessage(TGetListOutputDto entity)
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
    }
}
