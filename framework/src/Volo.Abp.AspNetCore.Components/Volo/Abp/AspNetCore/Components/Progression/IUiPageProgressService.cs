using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Progression;

public interface IUiPageProgressService
{
    /// <summary>
    /// An event raised after the notification is received.
    /// </summary>
    public event EventHandler<UiPageProgressEventArgs> ProgressChanged;

    /// <summary>
    /// Sets the progress percentage.
    /// </summary>
    /// <param name="percentage">Value of the progress from 0 to 100, or null for indeterminate progress.</param>
    /// <param name="options">Additional options.</param>
    /// <returns>Awaitable task.</returns>
    Task Go(int? percentage, Action<UiPageProgressOptions> options = null);
}
