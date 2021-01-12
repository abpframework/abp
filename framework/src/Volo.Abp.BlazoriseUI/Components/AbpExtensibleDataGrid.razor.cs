using System;
using Blazorise.Extensions;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.AspNetCore.Components.Extensibility.TableColumns;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class AbpExtensibleDataGrid<TItem> : ComponentBase
    {
        protected Dictionary<string, DataGridEntityActionsColumn<TItem>> ActionColumns =
            new Dictionary<string, DataGridEntityActionsColumn<TItem>>();

        [Parameter] public IEnumerable<TItem> Data { get; set; }

        [Parameter] public EventCallback<DataGridReadDataEventArgs<TItem>> ReadData { get; set; }

        [Parameter] public int? TotalItems { get; set; }

        [Parameter] public bool ShowPager { get; set; }

        [Parameter] public int PageSize { get; set; }

        [Parameter] public IEnumerable<TableColumn> Columns { get; set; }

        protected RenderFragment RenderCustomTableColumnComponent(Type type, object data)
        {
            return (builder) =>
            {
                builder.OpenComponent(type);
                builder.AddAttribute(0, "Data", data);
                builder.CloseComponent();
            };
        }
    }
}