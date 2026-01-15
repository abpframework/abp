using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Progression;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class UiPageProgress : ComponentBase, IDisposable
{
    protected int? Percentage { get; set; }

    protected bool Visible { get; set; }

    protected Color Color { get; set; } = Color.Primary;

    [Inject] protected IUiPageProgressService? UiPageProgressService { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (UiPageProgressService != null)
        {
            UiPageProgressService.ProgressChanged += OnProgressChanged;
        }
    }

    private async void OnProgressChanged(object? sender, UiPageProgressEventArgs e)
    {
        Percentage = e.Percentage;
        Visible = e.Percentage == null || (e.Percentage >= 0 && e.Percentage <= 100);
        Color = GetColor(e.Options.Type);

        await InvokeAsync(StateHasChanged);
    }

    public virtual void Dispose()
    {
        if (UiPageProgressService != null)
        {
            UiPageProgressService.ProgressChanged -= OnProgressChanged;
        }
    }

    protected virtual Color GetColor(UiPageProgressType pageProgressType)
    {
        return pageProgressType switch
        {
            UiPageProgressType.Info => Color.Info,
            UiPageProgressType.Success => Color.Success,
            UiPageProgressType.Warning => Color.Warning,
            UiPageProgressType.Error => Color.Error,
            _ => Color.Primary,
        };
    }
}
