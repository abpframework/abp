using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp;

/// <summary>
/// This class can be used to provide an action when
/// DisposeAsync method is called.
/// </summary>
public class AsyncDisposeFunc : IAsyncDisposable
{
    private readonly Func<Task> _func;

    /// <summary>
    /// Creates a new <see cref="AsyncDisposeFunc"/> object.
    /// </summary>
    /// <param name="func">func to be executed when this object is DisposeAsync.</param>
    public AsyncDisposeFunc([NotNull] Func<Task> func)
    {
        Check.NotNull(func, nameof(func));

        _func = func;
    }

    public async ValueTask DisposeAsync()
    {
        await _func();
    }
}
