using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Progression;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MudBlazorUI;

[Dependency(ReplaceServices = true)]
public class MudBlazorUiPageProgressService : IUiPageProgressService, IScopedDependency, IDisposable
{
    /// <summary>
    /// An event raised after the progress is changed.
    /// </summary>
    public event EventHandler<UiPageProgressEventArgs>? ProgressChanged;

    protected virtual int HideDelayMs => 250;

    private int _activeCount;
    private UiPageProgressOptions _lastOptions = new();
    private readonly Timer _hideTimer;

    public MudBlazorUiPageProgressService()
    {
        _hideTimer = new Timer(_ => ProgressChanged?.Invoke(this, new UiPageProgressEventArgs(-1, _lastOptions)));
    }

    public Task Go(int? percentage, Action<UiPageProgressOptions>? options = null)
    {
        var opt = new UiPageProgressOptions();
        options?.Invoke(opt);
        _lastOptions = opt;

        if (percentage == -1 && Interlocked.Decrement(ref _activeCount) <= 0)
        {
            Interlocked.Exchange(ref _activeCount, 0);
            _hideTimer.Change(HideDelayMs, Timeout.Infinite);
        }
        else if (percentage == null && Interlocked.Increment(ref _activeCount) == 1)
        {
            _hideTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProgressChanged?.Invoke(this, new UiPageProgressEventArgs(null, opt));
        }

        return Task.CompletedTask;
    }

    public void Dispose() => _hideTimer.Dispose();
}
