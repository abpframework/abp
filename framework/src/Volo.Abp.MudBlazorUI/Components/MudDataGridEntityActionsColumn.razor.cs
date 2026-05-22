using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class MudDataGridEntityActionsColumn<TItem> : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string Width { get; set; } = "150px";

    [Parameter]
    public bool Displayable { get; set; } = true;

    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetDefaultValuesAsync();
    }

    protected virtual ValueTask SetDefaultValuesAsync()
    {
        Title ??= UiLocalizer["Actions"];
        return ValueTask.CompletedTask;
    }
}
