using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus;

public class EventHandlerMethodExecutor
{
    private readonly MethodExecutorAsync _executorAsync;
    private delegate Task MethodExecutorAsync(IEventHandler target, object[] parameters);

    public MethodInfo MethodInfo { get; }

    public TypeInfo TargetTypeInfo { get; }

    private EventHandlerMethodExecutor(MethodInfo methodInfo, TypeInfo targetTypeInfo)
    {
        if (methodInfo == null)
        {
            throw new ArgumentNullException(nameof(methodInfo));
        }

        MethodInfo = methodInfo;
        TargetTypeInfo = targetTypeInfo;

        _executorAsync = GetExecutorAsync(methodInfo, targetTypeInfo);
    }

    private static MethodExecutorAsync GetExecutorAsync(MethodInfo methodInfo, TypeInfo targetTypeInfo)
    {
        var targetParameter = Expression.Parameter(typeof(IEventHandler), "target");
        var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

        var paramInfos = methodInfo.GetParameters();
        var parameters = new List<Expression>(paramInfos.Length);

        for (var i = 0; i < paramInfos.Length; i++)
        {
            var paramInfo = paramInfos[i];
            var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
            var valueCast = Expression.Convert(valueObj, paramInfo.ParameterType);

            parameters.Add(valueCast);
        }

        var instanceCast = Expression.Convert(targetParameter, targetTypeInfo.AsType());
        var methodCall = Expression.Call(instanceCast, methodInfo, parameters);

        var castMethodCall = Expression.Convert(methodCall, typeof(Task));
        var lambda = Expression.Lambda<MethodExecutorAsync>(castMethodCall, targetParameter, parametersParameter);
        return lambda.Compile();

    }

    public static EventHandlerMethodExecutor Create(MethodInfo methodInfo, TypeInfo targetTypeInfo)
    {
        return new EventHandlerMethodExecutor(methodInfo, targetTypeInfo);
    }

    public Task ExecuteAsync(IEventHandler target, object[] parameters)
    {
        return _executorAsync(target, parameters);
    }
}
