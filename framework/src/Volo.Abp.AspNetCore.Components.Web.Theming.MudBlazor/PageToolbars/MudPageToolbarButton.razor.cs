using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

public partial class MudPageToolbarButton : ComponentBase
{
    [Parameter]
    public Color Color { get; set; } = Color.Primary;

    [Parameter]
    public object? Icon { get; set; }

    [Parameter]
    public string Text { get; set; } = default!;

    [Parameter]
    public Func<Task>? OnClickAsync { get; set; }

    [Parameter]
    public bool Disabled { get; set; }
}
