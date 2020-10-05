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
        : OwningComponentBase
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
        [Inject] protected IUiMessageService UiMessageService { get; set; }
        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }
        [Inject] protected IAuthorizationService AuthorizationService { get; set; }

        protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        protected int CurrentPage;
        protected string CurrentSorting;
        protected int? TotalCount;
        protected IReadOnlyList<TListViewModel> Entities = Array.Empty<TListViewModel>();
        protected TCreateViewModel NewEntity;
        protected TKey EditingEntityId;
        protected TUpdateViewModel EditingEntity;
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

        protected AbpCrudPageBase()
        {
            NewEntity = new TCreateViewModel();
            EditingEntity = new TUpdateViewModel();
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
            Entities = MapToListViewModel(result.Items);
            TotalCount = (int?) result.TotalCount;
        }

        private IReadOnlyList<TListViewModel> MapToListViewModel(IReadOnlyList<TGetListOutputDto> dtos)
        {
            if (typeof(TGetListOutputDto) == typeof(TListViewModel))
            {
                return dtos.As<IReadOnlyList<TListViewModel>>();
            }

            return ObjectMapper.Map<IReadOnlyList<TGetListOutputDto>, List<TListViewModel>>(dtos);
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

        protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TListViewModel> e)
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

            NewEntity = new TCreateViewModel();
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
            await CheckCreatePolicyAsync();
            var createInput = MapToCreateInput(NewEntity);
            await AppService.CreateAsync(createInput);
            await GetEntitiesAsync();
            CreateModal.Hide();
        }



        protected virtual async Task UpdateEntityAsync()
        {
            await CheckUpdatePolicyAsync();
            var updateInput = MapToUpdateInput(EditingEntity);
            await AppService.UpdateAsync(EditingEntityId, updateInput);
            await GetEntitiesAsync();
            EditModal.Hide();
        }

        protected virtual async Task DeleteEntityAsync(TListViewModel entity)
        {
            await CheckDeletePolicyAsync();

            if (!await UiMessageService.ConfirmAsync(GetDeleteConfirmationMessage(entity)))
            {
                return;
            }

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
    }
}
