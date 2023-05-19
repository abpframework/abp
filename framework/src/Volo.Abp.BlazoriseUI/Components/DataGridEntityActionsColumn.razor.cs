using System.Linq;
using System.Threading.Tasks;
using Blazorise.DataGrid;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.BlazoriseUI.Components;

public partial class DataGridEntityActionsColumn<TItem> : DataGridColumn<TItem>
{
    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetDefaultValuesAsync();
    }

    protected virtual ValueTask SetDefaultValuesAsync()
    {
        Caption = UiLocalizer["Actions"];
        Width = "150px";
        Sortable = false;
        Field = typeof(TItem).GetProperties().First().Name;
        return ValueTask.CompletedTask;
    }
}
