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
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Authorization;
using Volo.Abp.BlazoriseUI.Components.ObjectExtending;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.BlazoriseUI
{
    public abstract class AbpReadOnlyPageBase<
            TAppService,
            TEntityDto,
            TKey>
        : AbpReadOnlyPageBase<
            TAppService,
            TEntityDto,
            TKey,
            PagedAndSortedResultRequestDto>
        where TAppService : IReadOnlyAppService<
            TEntityDto,
            TKey,
            PagedAndSortedResultRequestDto>
        where TEntityDto : class, IEntityDto<TKey>, new()
    {
    }

    public abstract class AbpReadOnlyPageBase<
            TAppService,
            TEntityDto,
            TKey,
            TGetListInput>
        : AbpComponentBase
        where TAppService : IReadOnlyAppService<
            TEntityDto,
            TKey,
            TGetListInput>
        where TEntityDto : class, IEntityDto<TKey>, new()
        where TGetListInput : PagedAndSortedResultRequestDto, new()
    {
        [Inject] protected TAppService AppService { get; set; }
        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

        protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        protected int CurrentPage = 1;
        protected string CurrentSorting;
        protected int? TotalCount;
        protected TGetListInput GetListInput = new TGetListInput();
        protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
        protected TableColumnDictionary TableColumns { get; set; }
        protected IReadOnlyList<TEntityDto> Entities = Array.Empty<TEntityDto>();


        protected AbpReadOnlyPageBase()
        {
            TableColumns = new TableColumnDictionary();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetTableColumnsAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                await SetToolbarItemsAsync();
                await SetBreadcrumbItemsAsync();
            }
        }

        protected virtual async Task GetEntitiesAsync()
        {
            try
            {
                await UpdateGetListInputAsync();
                var result = await AppService.GetListAsync(GetListInput);
                Entities = result.Items;
                TotalCount = (int?)result.TotalCount;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        protected virtual async Task SearchEntitiesAsync()
        {
            CurrentPage = 1;

            await GetEntitiesAsync();

            await InvokeAsync(StateHasChanged);
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

        protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TEntityDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;

            await GetEntitiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
        /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
        ///
        /// Does nothing if <paramref name="policyName"/> is null or empty.
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

        protected virtual ValueTask SetTableColumnsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual IEnumerable<TableColumn> GetExtensionTableColumns(string moduleName, string entityType)
        {
            var properties = ModuleExtensionConfigurationHelper.GetPropertyConfigurations(moduleName, entityType);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.IsAvailableToClients && propertyInfo.UI.OnTable.IsVisible)
                {
                    if (propertyInfo.Name.EndsWith("_Text"))
                    {
                        var lookupPropertyName = propertyInfo.Name.RemovePostFix("_Text");
                        var lookupPropertyDefinition = properties.SingleOrDefault(t => t.Name == lookupPropertyName);
                        yield return new TableColumn
                        {
                            Title = lookupPropertyDefinition.GetLocalizedDisplayName(StringLocalizerFactory),
                            Data = $"ExtraProperties[{propertyInfo.Name}]"
                        };
                    }
                    else
                    {
                        var column = new TableColumn
                        {
                            Title = propertyInfo.GetLocalizedDisplayName(StringLocalizerFactory),
                            Data = $"ExtraProperties[{propertyInfo.Name}]"
                        };

                        if (propertyInfo.IsDate() || propertyInfo.IsDateTime())
                        {
                            column.DisplayFormat = BlazoriseUiObjectExtensionPropertyInfoExtensions.GetDateEditInputFormatOrNull(propertyInfo);
                        }

                        if (propertyInfo.Type.IsEnum)
                        {
                            column.ValueConverter = (val) =>
                                EnumHelper.GetLocalizedMemberName(propertyInfo.Type,
                                    val.As<ExtensibleObject>().ExtraProperties[propertyInfo.Name],
                                    StringLocalizerFactory);
                        }

                        yield return column;
                    }
                }
            }
        }

    }
}