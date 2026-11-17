using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Reflection;

namespace Volo.Abp.DynamicProxy;

public static class AbpMethodInvocationAsyncEnumerableExtensions
{
    private delegate object WrapHandler(object source, Func<IAbpInterceptionScope> beginScope);

    private static readonly MethodInfo WrapMethod = typeof(AbpMethodInvocationAsyncEnumerableExtensions)
        .GetMethod(nameof(Wrap), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static readonly ConcurrentDictionary<MethodInfo, WrapHandler?> WrapHandlers = new();

    /// <summary>
    /// Runs each enumeration of a returned <see cref="IAsyncEnumerable{T}"/> in a new scope.
    /// <paramref name="beginScope"/> must set its async local values (e.g. the ambient unit of work) synchronously.
    /// </summary>
    public static bool WrapAsyncEnumerable(this IAbpMethodInvocation invocation, Func<IAbpInterceptionScope> beginScope)
    {
        var handler = WrapHandlers.GetOrAdd(invocation.Method, CreateHandler);
        if (handler == null || invocation.ReturnValue == null)
        {
            return false;
        }

        invocation.ReturnValue = handler(invocation.ReturnValue, beginScope);
        return true;
    }

    private static WrapHandler? CreateHandler(MethodInfo method)
    {
        // The return value of a method with ref or out parameters cannot be replaced, as it is not intercepted asynchronously.
        var returnType = method.ReturnType;
        if (!TypeHelper.IsAsyncEnumerable(returnType) ||
            method.GetParameters().Any(p => p.ParameterType.IsByRef))
        {
            return null;
        }

        return (WrapHandler)WrapMethod
            .MakeGenericMethod(returnType.GenericTypeArguments[0])
            .CreateDelegate(typeof(WrapHandler));
    }

    private static object Wrap<T>(object source, Func<IAbpInterceptionScope> beginScope)
    {
        return EnumerateInScopeAsync((IAsyncEnumerable<T>)source, beginScope);
    }

    private static async IAsyncEnumerable<T> EnumerateInScopeAsync<T>(
        IAsyncEnumerable<T> source,
        Func<IAbpInterceptionScope> beginScope,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var scope = beginScope();

        // Each step of an async iterator runs in the execution context of its caller, without the async local values set by beginScope.
        var context = CaptureExecutionContext();

        try
        {
            IAsyncEnumerator<T> enumerator;
            try
            {
                enumerator = Run(context, () => source.GetAsyncEnumerator(cancellationToken));
            }
            catch (Exception exception)
            {
                await Run(context, () => scope.FailAsync(exception));
                throw;
            }

            Exception? sourceException = null;

            try
            {
                while (true)
                {
                    bool hasNext;
                    T current = default!;
                    try
                    {
                        hasNext = await Run(context, () => MoveNextAsync(enumerator));
                        if (hasNext)
                        {
                            current = Run(context, () => enumerator.Current);
                        }
                    }
                    catch (Exception exception)
                    {
                        sourceException = exception;
                        throw;
                    }

                    if (!hasNext)
                    {
                        break;
                    }

                    yield return current;
                }
            }
            finally
            {
                // The source is disposed first, so the scope covers its cleanup.
                try
                {
                    await Run(context, () => DisposeAsync(enumerator));
                }
                catch (Exception exception)
                {
                    // It replaces the exception of the source, if any, as the one thrown to the caller.
                    sourceException = exception;
                    throw;
                }
                finally
                {
                    if (sourceException != null)
                    {
                        await Run(context, () => scope.FailAsync(sourceException));
                    }
                    else
                    {
                        // Also when the caller stops early, like a method that has returned.
                        await Run(context, () => scope.CompleteAsync(cancellationToken));
                    }
                }
            }
        }
        finally
        {
            Run(context, () =>
            {
                scope.Dispose();
                return true;
            });
        }
    }

    private static ExecutionContext CaptureExecutionContext()
    {
        // Capture returns null while the flow is suppressed. Restoring it does not affect the caller,
        // whose execution context is restored when this step of the async iterator returns.
        if (ExecutionContext.IsFlowSuppressed())
        {
            ExecutionContext.RestoreFlow();
        }

        return ExecutionContext.Capture()!;
    }

    // The value task of the source (e.g. an IValueTaskSource) can also run its code when completed and when its result is read.
    private static async ValueTask<bool> MoveNextAsync<T>(IAsyncEnumerator<T> enumerator)
    {
        return await enumerator.MoveNextAsync();
    }

    private static async ValueTask DisposeAsync<T>(IAsyncEnumerator<T> enumerator)
    {
        await enumerator.DisposeAsync();
    }

    private static TResult Run<TResult>(ExecutionContext context, Func<TResult> func)
    {
        TResult result = default!;
        ExecutionContext.Run(context.CreateCopy(), _ => result = func(), null);
        return result;
    }
}
