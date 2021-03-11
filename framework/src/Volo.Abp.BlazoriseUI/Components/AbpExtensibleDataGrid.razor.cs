using System;
using Blazorise.Extensions;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.AspNetCore.Components.Extensibility.TableColumns;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Extensibility.EntityActions;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class AbpExtensibleDataGrid<TItem> : ComponentBase
    {
        protected const string DataFieldAttributeName = "Data";

        protected Dictionary<string, DataGridEntityActionsColumn<TItem>> ActionColumns =
            new Dictionary<string, DataGridEntityActionsColumn<TItem>>();

        protected Regex ExtensionPropertiesRegex = new Regex(@"ExtraProperties\[(.*?)\]");

        [Parameter] public IEnumerable<TItem> Data { get; set; }

        [Parameter] public EventCallback<DataGridReadDataEventArgs<TItem>> ReadData { get; set; }

        [Parameter] public int? TotalItems { get; set; }

        [Parameter] public bool ShowPager { get; set; }

        [Parameter] public int PageSize { get; set; }

        [Parameter] public IEnumerable<TableColumn> Columns { get; set; }

        [Parameter] public int CurrentPage { get; set; }

        [Inject]
        public IStringLocalizerFactory StringLocalizerFactory { get; set; }

        protected virtual RenderFragment RenderCustomTableColumnComponent(Type type, object data)
        {
            return (builder) =>
            {
                builder.OpenComponent(type);
                builder.AddAttribute(0, DataFieldAttributeName, data);
                builder.CloseComponent();
            };
        }

        protected Task<bool> VisibleCore(EntityAction action, TItem item)
        {
            if (action.Visible != null)
            {
                return action.Visible.Invoke(item);
            }
            else
            {
                return Task.FromResult(true);
            }
        }
    }
}