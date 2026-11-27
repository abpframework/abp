using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Reflection;

namespace Volo.Abp.Castle.DynamicProxy;

internal static class CastleAsyncEnumerableAbpInterceptor
{
    private delegate object AsyncEnumerableHandler(IInvocation invocation, IAbpInterceptor interceptor);

    private static readonly MethodInfo InterceptMethod = typeof(CastleAsyncEnumerableAbpInterceptor)
        .GetMethod(nameof(Intercept), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static readonly ConcurrentDictionary<Type, AsyncEnumerableHandler> Handlers = new();

    public static bool TryIntercept(IInvocation invocation, IAbpInterceptor interceptor)
    {
        var returnType = invocation.Method.ReturnType;
        if (!TypeHelper.IsAsyncEnumerable(returnType))
        {
            return false;
        }

        if (CastleByRefParameterChecker.HasByRefParameters(invocation.Method))
        {
            return false;
        }

        invocation.ReturnValue = Handlers.GetOrAdd(returnType, CreateHandler)(invocation, interceptor);
        return true;
    }

    private static AsyncEnumerableHandler CreateHandler(Type returnType)
    {
        return (AsyncEnumerableHandler)InterceptMethod
            .MakeGenericMethod(returnType.GetGenericArguments()[0])
            .CreateDelegate(typeof(AsyncEnumerableHandler));
    }

    private static object Intercept<T>(IInvocation invocation, IAbpInterceptor interceptor)
    {
        var methodInvocation = new CastleAbpMethodInvocationAdapterWithReturnValue<IAsyncEnumerable<T>>(
            invocation,
            invocation.CaptureProceedInfo(),
            ProceedAsync<T>);

        return new InterceptedAsyncEnumerable<T>(InterceptAsync(interceptor, methodInvocation), methodInvocation);
    }

    private static async Task InterceptAsync(IAbpInterceptor interceptor, IAbpMethodInvocation methodInvocation)
    {
        // Also captures a synchronous exception of the interceptor, so it is thrown when enumerating, like for a Task.
        await interceptor.InterceptAsync(methodInvocation);
    }

    private static async Task<IAsyncEnumerable<T>> ProceedAsync<T>(IInvocation invocation, IInvocationProceedInfo proceedInfo)
    {
        proceedInfo.Invoke();

        // Waits for the next interceptor, like awaiting the task of a Task-returning method.
        if (invocation.ReturnValue is InterceptedAsyncEnumerable<T> next)
        {
            return (await next.GetResultAsync())!;
        }

        return (IAsyncEnumerable<T>)invocation.ReturnValue!;
    }

    private class InterceptedAsyncEnumerable<T> : IAsyncEnumerable<T>, IAbpInterceptedAsyncEnumerable
    {
        private readonly Task _interceptTask;
        private readonly IAbpMethodInvocation _methodInvocation;

        public InterceptedAsyncEnumerable(Task interceptTask, IAbpMethodInvocation methodInvocation)
        {
            _interceptTask = interceptTask;
            _methodInvocation = methodInvocation;
        }

        public async Task<IAsyncEnumerable<T>?> GetResultAsync()
        {
            await _interceptTask;
            return (IAsyncEnumerable<T>?)_methodInvocation.ReturnValue;
        }

        public Task WaitForInterceptorsAsync()
        {
            return GetResultAsync();
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return EnumerateAsync(cancellationToken).GetAsyncEnumerator(cancellationToken);
        }

        private async IAsyncEnumerable<T> EnumerateAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // Keeps the execution context of the caller for the await below even if the caller suppresses its flow.
            // The caller is not affected, as its execution context is restored when this step of the async iterator returns.
            if (ExecutionContext.IsFlowSuppressed())
            {
                ExecutionContext.RestoreFlow();
            }

            var source = await GetResultAsync();
            if (source == null)
            {
                yield break;
            }

            await foreach (var item in source.WithCancellation(cancellationToken))
            {
                yield return item;
            }
        }
    }
}
