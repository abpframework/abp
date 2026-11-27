using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy;

internal static class CastleValueTaskAbpInterceptor
{
    private delegate object ValueTaskHandler(IInvocation invocation, IAbpInterceptor interceptor);

    private static readonly MethodInfo InterceptWithResultMethod = typeof(CastleValueTaskAbpInterceptor)
        .GetMethod(nameof(InterceptWithResult), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static readonly ConcurrentDictionary<Type, ValueTaskHandler> Handlers = new();

    public static bool TryIntercept(IInvocation invocation, IAbpInterceptor interceptor)
    {
        var returnType = invocation.Method.ReturnType;
        var isValueTask = returnType == typeof(ValueTask);
        if (!isValueTask && !(returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>)))
        {
            return false;
        }

        if (CastleByRefParameterChecker.HasByRefParameters(invocation.Method))
        {
            return false;
        }

        invocation.ReturnValue = isValueTask
            ? Intercept(invocation, interceptor)
            : Handlers.GetOrAdd(returnType, CreateHandler)(invocation, interceptor);

        return true;
    }

    private static ValueTaskHandler CreateHandler(Type returnType)
    {
        return (ValueTaskHandler)InterceptWithResultMethod
            .MakeGenericMethod(returnType.GetGenericArguments()[0])
            .CreateDelegate(typeof(ValueTaskHandler));
    }

    private static object Intercept(IInvocation invocation, IAbpInterceptor interceptor)
    {
        var methodInvocation = new CastleAbpMethodInvocationAdapter(
            invocation,
            invocation.CaptureProceedInfo(),
            ProceedAsync);

        return new ValueTask(InterceptAsync(interceptor, methodInvocation));
    }

    private static object InterceptWithResult<TResult>(IInvocation invocation, IAbpInterceptor interceptor)
    {
        var methodInvocation = new CastleAbpMethodInvocationAdapterWithReturnValue<TResult>(
            invocation,
            invocation.CaptureProceedInfo(),
            ProceedAsync<TResult>);

        return new ValueTask<TResult>(InterceptAsync<TResult>(interceptor, methodInvocation));
    }

    private static async Task InterceptAsync(IAbpInterceptor interceptor, IAbpMethodInvocation methodInvocation)
    {
        // Also captures a synchronous exception of the interceptor, so it is thrown when awaiting, like for a Task.
        await interceptor.InterceptAsync(methodInvocation);
    }

    private static async Task<TResult> InterceptAsync<TResult>(IAbpInterceptor interceptor, IAbpMethodInvocation methodInvocation)
    {
        await interceptor.InterceptAsync(methodInvocation);
        return (TResult)methodInvocation.ReturnValue;
    }

    private static async Task ProceedAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo)
    {
        proceedInfo.Invoke();
        await (ValueTask)invocation.ReturnValue!;
    }

    private static async Task<TResult> ProceedAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo)
    {
        proceedInfo.Invoke();
        return await (ValueTask<TResult>)invocation.ReturnValue!;
    }
}
