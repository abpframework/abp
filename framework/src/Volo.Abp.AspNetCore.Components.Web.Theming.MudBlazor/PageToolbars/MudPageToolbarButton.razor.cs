using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using global::MudBlazor;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

public partial class MudPageToolbarButton : ComponentBase
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public object? Icon { get; set; }

    [Parameter]
    public Func<Task>? Clicked { get; set; }

    private EventCallback<MouseEventArgs> OnClickCallback => EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAsync);

    private async Task OnClickAsync(MouseEventArgs args)
    {
        if (Clicked != null && !Disabled)
        {
            await Clicked.Invoke();
        }
        await Task.CompletedTask;
    }
}
