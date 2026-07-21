using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.BlazoriseUI.Components;

public partial class DataGridEntityActionsColumn<TItem> : DataGridColumn<TItem>
{
    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetDefaultValuesAsync();
    }

    protected virtual ValueTask SetDefaultValuesAsync()
    {
        Caption = UiLocalizer["Actions"];
        Width = Blazorise.Width.Px(150);
        Sortable = false;
        Field = ResolveFieldName();

        return ValueTask.CompletedTask;
    }

    protected virtual string ResolveFieldName()
    {
        var props = typeof(TItem).GetProperties();
        return props.Length > 0
            ? props[0].Name
            : "Id";
    }
}
