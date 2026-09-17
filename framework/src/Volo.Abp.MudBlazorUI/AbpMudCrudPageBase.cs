using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.MudBlazorUI.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.MudBlazorUI;

public abstract class AbpMudCrudPageBase<
        TAppService,
        TEntityDto,
        TKey>
    : AbpMudCrudPageBase<
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

public abstract class AbpMudCrudPageBase<
        TAppService,
        TEntityDto,
        TKey,
        TGetListInput>
    : AbpMudCrudPageBase<
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

public abstract class AbpMudCrudPageBase<
        TAppService,
        TEntityDto,
        TKey,
        TGetListInput,
        TCreateInput>
    : AbpMudCrudPageBase<
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

public abstract class AbpMudCrudPageBase<
        TAppService,
        TEntityDto,
        TKey,
        TGetListInput,
        TCreateInput,
        TUpdateInput>
    : AbpMudCrudPageBase<
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

public abstract class AbpMudCrudPageBase<
        TAppService,
        TGetOutputDto,
        TGetListOutputDto,
        TKey,
        TGetListInput,
        TCreateInput,
        TUpdateInput>
    : AbpMudCrudPageBase<
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

public abstract class AbpMudCrudPageBase<
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
    [Inject] protected TAppService AppService { get; set; } = default!;
    [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;
    [Inject] public IAbpEnumLocalizer AbpEnumLocalizer { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected ExtensionPropertyPolicyChecker ExtensionPropertyPolicyChecker { get; set; } = default!;

    protected virtual int PageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;
    protected string CurrentSorting = default!;
    protected TGetListInput GetListInput = new TGetListInput();
    protected TCreateViewModel NewEntity;
    protected TKey EditingEntityId = default!;
    protected TUpdateViewModel EditingEntity;
    protected MudForm? CreateFormRef;
    protected MudForm? EditFormRef;
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
    protected MudDataGridEntityActionsColumn<TListViewModel> EntityActionsColumn = default!;
    protected EntityActionDictionary EntityActions { get; set; }
    protected TableColumnDictionary TableColumns { get; set; }

    protected string? CreatePolicyName { get; set; }
    protected string? UpdatePolicyName { get; set; }
    protected string? DeletePolicyName { get; set; }

    public bool HasCreatePermission { get; set; }
    public bool HasUpdatePermission { get; set; }
    public bool HasDeletePermission { get; set; }

    protected MudDialog? _createDialog;
    protected MudDialog? _editDialog;

    protected bool IsCreating { get; set; }
    protected bool IsUpdating { get; set; }

    protected virtual DialogOptions CreateDialogOptions => new DialogOptions
    {
        MaxWidth = MaxWidth.Medium,
        FullWidth = true,
        CloseOnEscapeKey = true,
        BackdropClick = false
    };

    protected virtual DialogOptions EditDialogOptions => new DialogOptions
    {
        MaxWidth = MaxWidth.Medium,
        FullWidth = true,
        CloseOnEscapeKey = true,
        BackdropClick = false
    };

    protected AbpMudExtensibleDataGrid<TListViewModel> _dataGrid = default!;

    protected AbpMudCrudPageBase()
    {
        NewEntity = new TCreateViewModel();
        EditingEntity = new TUpdateViewModel();
        TableColumns = new TableColumnDictionary();
        EntityActions = new EntityActionDictionary();
    }

    protected override async Task OnInitializedAsync()
    {
        await TrySetPermissionsAsync();
        await TrySetEntityActionsAsync();
        await TrySetTableColumnsAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task TrySetPermissionsAsync()
    {
        if (IsDisposed)
        {
            return;
        }

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

    private IReadOnlyList<TListViewModel> MapToListViewModel(IReadOnlyList<TGetListOutputDto> dtos)
    {
        if (typeof(TGetListOutputDto) == typeof(TListViewModel))
        {
            return dtos.As<IReadOnlyList<TListViewModel>>();
        }

        return ObjectMapper.Map<IReadOnlyList<TGetListOutputDto>, List<TListViewModel>>(dtos);
    }

    protected virtual string MapSortFieldToPropertyName(string sortBy, List<TableColumn> columns)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            return sortBy;
        }

        return sortBy;
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
        var currentPage = CurrentPage;
        CurrentPage = 1;
        if (currentPage == 1)
        {
            await _dataGrid.ReloadServerDataAsync();
        }
        else
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual async Task<GridData<TListViewModel>> OnDataGridReadAsync(GridState<TListViewModel> state, CancellationToken cancellationToken = default)
    {
        CurrentSorting = state.SortDefinitions
            .Select(s => _dataGrid.ResolveSortPropertyName(s) + (s.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = state.Page + 1;
        if (state.PageSize > 0)
        {
            PageSize = state.PageSize;
        }

        try
        {
            await UpdateGetListInputAsync();
            var result = await AppService.GetListAsync(GetListInput);
            var items = MapToListViewModel(result.Items);

            await InvokeAsync(StateHasChanged);

            return new GridData<TListViewModel>
            {
                Items = items,
                TotalItems = (int)result.TotalCount
            };
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            return new GridData<TListViewModel>
            {
                Items = Array.Empty<TListViewModel>(),
                TotalItems = 0
            };
        }
    }

    protected virtual async Task OpenCreateDialogAsync()
    {
        try
        {
            await CheckCreatePolicyAsync();

            NewEntity = new TCreateViewModel();

            if (_createDialog != null)
            {
                await InvokeAsync(() => _createDialog.ShowAsync());
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task CloseCreateDialogAsync()
    {
        NewEntity = new TCreateViewModel();
        if (_createDialog != null)
        {
            return InvokeAsync(() => _createDialog.CloseAsync());
        }
        return Task.CompletedTask;
    }

    protected virtual async Task OpenEditDialogAsync(TListViewModel entity)
    {
        try
        {
            await CheckUpdatePolicyAsync();

            var entityDto = await AppService.GetAsync(entity.Id);

            EditingEntityId = entity.Id;
            EditingEntity = MapToEditingEntity(entityDto);

            if (_editDialog != null)
            {
                await InvokeAsync(() => _editDialog.ShowAsync());
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
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

    protected virtual Task CloseEditDialogAsync()
    {
        if (_editDialog != null)
        {
            return InvokeAsync(() => _editDialog.CloseAsync());
        }
        return Task.CompletedTask;
    }

    protected virtual async Task CreateEntityAsync()
    {
        if (IsCreating)
        {
            return;
        }

        try
        {
            IsCreating = true;
            await InvokeAsync(StateHasChanged);

            var isValid = true;
            if (CreateFormRef != null)
            {
                await CreateFormRef.ValidateAsync();
                isValid = CreateFormRef.IsValid;
            }

            if (isValid)
            {
                await OnCreatingEntityAsync();

                await CheckCreatePolicyAsync();
                var createInput = MapToCreateInput(NewEntity);
                await AppService.CreateAsync(createInput);

                await OnCreatedEntityAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        finally
        {
            IsCreating = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual Task OnCreatingEntityAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual async Task OnCreatedEntityAsync()
    {
        await _dataGrid.ReloadServerDataAsync();
        await CloseCreateDialogAsync();
        Snackbar.Add(GetCreateMessage(), Severity.Success);
    }

    protected virtual string GetCreateMessage()
    {
        return UiLocalizer["CreatedSuccessfully"];
    }

    protected virtual async Task UpdateEntityAsync()
    {
        if (IsUpdating)
        {
            return;
        }

        try
        {
            IsUpdating = true;
            await InvokeAsync(StateHasChanged);

            var isValid = true;
            if (EditFormRef != null)
            {
                await EditFormRef.ValidateAsync();
                isValid = EditFormRef.IsValid;
            }

            if (isValid)
            {
                await OnUpdatingEntityAsync();

                await CheckUpdatePolicyAsync();
                var updateInput = MapToUpdateInput(EditingEntity);
                await AppService.UpdateAsync(EditingEntityId, updateInput);

                await OnUpdatedEntityAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        finally
        {
            IsUpdating = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual Task OnUpdatingEntityAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual async Task OnUpdatedEntityAsync()
    {
        await _dataGrid.ReloadServerDataAsync();
        await CloseEditDialogAsync();
        Snackbar.Add(GetUpdateMessage(), Severity.Success);
    }

    protected virtual string GetUpdateMessage()
    {
        return UiLocalizer["SavedSuccessfully"];
    }

    protected virtual async Task DeleteEntityAsync(TListViewModel entity)
    {
        try
        {
            await CheckDeletePolicyAsync();
            
            var confirmed = await ConfirmDeleteAsync(entity);
            if (!confirmed)
            {
                return;
            }

            await OnDeletingEntityAsync();
            await AppService.DeleteAsync(entity.Id);
            await OnDeletedEntityAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task OnDeletingEntityAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual async Task OnDeletedEntityAsync()
    {
        if (_dataGrid != null)
        {
            await InvokeAsync(() => _dataGrid.ReloadServerDataAsync());
        }
        Snackbar.Add(GetDeleteMessage(), Severity.Success);
    }

    protected virtual string GetDeleteMessage()
    {
        return UiLocalizer["DeletedSuccessfully"];
    }

    protected virtual string GetDeleteConfirmationMessage(TListViewModel entity)
    {
        return UiLocalizer["ItemWillBeDeletedMessage"];
    }

    protected virtual async Task<bool> ConfirmDeleteAsync(TListViewModel entity)
    {
        var result = await DialogService.ShowMessageBoxAsync(
            UiLocalizer["AreYouSure"],
            GetDeleteConfirmationMessage(entity),
            yesText: UiLocalizer["Yes"],
            cancelText: UiLocalizer["Cancel"]);

        return result == true;
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
    /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
    /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
    ///
    /// Does nothing if <paramref name="policyName"/> is null or empty.
    /// </summary>
    /// <param name="policyName">A policy name to check</param>
    protected virtual async Task CheckPolicyAsync(string? policyName)
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

    private async ValueTask TrySetEntityActionsAsync()
    {
        if (IsDisposed)
        {
            return;
        }

        await SetEntityActionsAsync();
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        return ValueTask.CompletedTask;
    }

    private async ValueTask TrySetTableColumnsAsync()
    {
        if (IsDisposed)
        {
            return;
        }

        await SetTableColumnsAsync();
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected virtual async Task<List<TableColumn>> GetExtensionTableColumnsAsync(string moduleName, string entityType)
    {
        var tableColumns = new List<TableColumn>();
        var properties = ModuleExtensionConfigurationHelper.GetPropertyConfigurations(moduleName, entityType).ToList();
        foreach (var propertyInfo in properties)
        {
            if (!await ExtensionPropertyPolicyChecker.CheckPolicyAsync(propertyInfo.Policy))
            {
                continue;
            }

            if (propertyInfo.IsAvailableToClients && propertyInfo.UI.OnTable.IsVisible)
            {
                if (propertyInfo.Name.EndsWith("_Text"))
                {
                    var lookupPropertyName = propertyInfo.Name.RemovePostFix("_Text");
                    var lookupPropertyDefinition = properties.SingleOrDefault(t => t.Name == lookupPropertyName)!;
                    tableColumns.Add(new TableColumn
                    {
                        Title = lookupPropertyDefinition.GetLocalizedDisplayName(StringLocalizerFactory),
                        Data = $"ExtraProperties[{propertyInfo.Name}]",
                        PropertyName = propertyInfo.Name
                    });
                }
                else
                {
                    var column = new TableColumn
                    {
                        Title = propertyInfo.GetLocalizedDisplayName(StringLocalizerFactory),
                        Data = $"ExtraProperties[{propertyInfo.Name}]",
                        PropertyName = propertyInfo.Name
                    };

                    if (propertyInfo.IsDate() || propertyInfo.IsDateTime())
                    {
                        column.DisplayFormat = propertyInfo.GetDateEditInputFormatOrNull();
                    }

                    if (propertyInfo.Type.IsEnum)
                    {
                        column.ValueConverter = (val) =>
                            AbpEnumLocalizer.GetString(propertyInfo.Type, val.As<ExtensibleObject>().ExtraProperties[propertyInfo.Name]!, new IStringLocalizer?[] { StringLocalizerFactory.CreateDefaultOrNull() });
                    }

                    tableColumns.Add(column);
                }
            }
        }

        return tableColumns;
    }
}
