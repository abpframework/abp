using System;

namespace Volo.Abp.Threading;

/// <summary>
/// Extension methods to make locking easier.
/// </summary>
public static class LockExtensions
{
    /// <summary>
    /// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
    /// </summary>
    /// <param name="source">Source object (to be locked)</param>
    /// <param name="action">Action (to be executed)</param>
    public static void Locking(this object source, Action action)
    {
        lock (source)
        {
            action();
        }
    }

    /// <summary>
    /// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the object (to be locked)</typeparam>
    /// <param name="source">Source object (to be locked)</param>
    /// <param name="action">Action (to be executed)</param>
    public static void Locking<T>(this T source, Action<T> action) where T : class
    {
        lock (source)
        {
            action(source);
        }
    }

    /// <summary>
    /// Executes given <paramref name="func"/> and returns it's value by locking given <paramref name="source"/> object.
    /// </summary>
    /// <typeparam name="TResult">Return type</typeparam>
    /// <param name="source">Source object (to be locked)</param>
    /// <param name="func">Function (to be executed)</param>
    /// <returns>Return value of the <paramref name="func"/></returns>
    public static TResult Locking<TResult>(this object source, Func<TResult> func)
    {
        lock (source)
        {
            return func();
        }
    }

    /// <summary>
    /// Executes given <paramref name="func"/> and returns it's value by locking given <paramref name="source"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the object (to be locked)</typeparam>
    /// <typeparam name="TResult">Return type</typeparam>
    /// <param name="source">Source object (to be locked)</param>
    /// <param name="func">Function (to be executed)</param>
    /// <returns>Return value of the <paramnref name="func"/></returns>
    public static TResult Locking<T, TResult>(this T source, Func<T, TResult> func) where T : class
    {
        lock (source)
        {
            return func(source);
        }
    }
}
