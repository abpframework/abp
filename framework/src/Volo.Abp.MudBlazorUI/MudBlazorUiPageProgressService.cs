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
    private bool _visible;
    private UiPageProgressOptions _lastOptions;
    private readonly Timer _hideTimer;

    public MudBlazorUiPageProgressService()
    {
        _lastOptions = CreateDefaultOptions();
        _hideTimer = new Timer(_ =>
        {
            _visible = false;
            ProgressChanged?.Invoke(this, new UiPageProgressEventArgs(-1, _lastOptions));
        });
    }

    public Task Go(int? percentage, Action<UiPageProgressOptions>? options = null)
    {
        var opt = CreateDefaultOptions();
        options?.Invoke(opt);
        _lastOptions = opt;

        if (percentage == -1)
        {
            _activeCount--;
            if (_activeCount <= 0)
            {
                _activeCount = 0;
                _hideTimer.Change(HideDelayMs, Timeout.Infinite);
            }
        }
        else if (percentage == null)
        {
            _activeCount++;
            _hideTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (!_visible)
            {
                _visible = true;
                ProgressChanged?.Invoke(this, new UiPageProgressEventArgs(null, opt));
            }
        }
        else
        {
            ProgressChanged?.Invoke(this, new UiPageProgressEventArgs(percentage, opt));
        }

        return Task.CompletedTask;
    }

    protected virtual UiPageProgressOptions CreateDefaultOptions()
    {
        return new UiPageProgressOptions();
    }

    public void Dispose() => _hideTimer.Dispose();
}
