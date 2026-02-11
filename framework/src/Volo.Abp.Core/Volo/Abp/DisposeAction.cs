using System;
using JetBrains.Annotations;

namespace Volo.Abp;

/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// </summary>
public class DisposeAction : IDisposable
{
    private readonly Action _action;

    /// <summary>
    /// Creates a new <see cref="DisposeAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposeAction([NotNull] Action action)
    {
        Check.NotNull(action, nameof(action));

        _action = action;
    }

    public void Dispose()
    {
        _action();
    }
}

/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// <typeparam name="T">The type of the parameter of the action.</typeparam>
/// </summary>
public class DisposeAction<T> : IDisposable
{
    private readonly Action<T> _action;

    private readonly T? _parameter;

    /// <summary>
    /// Creates a new <see cref="DisposeAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    /// <param name="parameter">The parameter of the action.</param>
    public DisposeAction(Action<T> action, T parameter)
    {
        Check.NotNull(action, nameof(action));

        _action = action;
        _parameter = parameter;
    }

    public void Dispose()
    {
        if (_parameter != null)
        {
            _action(_parameter);
        }
    }
}
