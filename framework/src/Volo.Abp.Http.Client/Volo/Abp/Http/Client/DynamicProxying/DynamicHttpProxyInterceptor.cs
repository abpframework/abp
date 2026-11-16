using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Client.Proxying;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying;

public class DynamicHttpProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
{

    // ReSharper disable once StaticMemberInGenericType
    protected static MethodInfo CallRequestAsyncMethod { get; }

    // ReSharper disable once StaticMemberInGenericType
    protected static MethodInfo CallRequestAsyncEnumerableMethod { get; }

    static DynamicHttpProxyInterceptor()
    {
        CallRequestAsyncMethod = typeof(DynamicHttpProxyInterceptor<TService>)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(m => m.Name == nameof(CallRequestAsync) && m.IsGenericMethodDefinition);

        CallRequestAsyncEnumerableMethod = typeof(DynamicHttpProxyInterceptor<TService>)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(m => m.Name == nameof(CallRequestAsyncEnumerable) && m.IsGenericMethodDefinition);
    }

    public ILogger<DynamicHttpProxyInterceptor<TService>> Logger { get; set; }
    protected DynamicHttpProxyInterceptorClientProxy<TService> InterceptorClientProxy { get; }
    protected AbpHttpClientOptions ClientOptions { get; }
    protected IProxyHttpClientFactory HttpClientFactory { get; }
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }
    protected IApiDescriptionFinder ApiDescriptionFinder { get; }

    public DynamicHttpProxyInterceptor(
        DynamicHttpProxyInterceptorClientProxy<TService> interceptorClientProxy,
        IOptions<AbpHttpClientOptions> clientOptions,
        IProxyHttpClientFactory httpClientFactory,
        IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider,
        IApiDescriptionFinder apiDescriptionFinder)
    {
        InterceptorClientProxy = interceptorClientProxy;
        HttpClientFactory = httpClientFactory;
        RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
        ApiDescriptionFinder = apiDescriptionFinder;
        ClientOptions = clientOptions.Value;

        Logger = NullLogger<DynamicHttpProxyInterceptor<TService>>.Instance;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
        {
            invocation.ReturnValue = CallRequestAsyncEnumerableMethod
                .MakeGenericMethod(returnType.GenericTypeArguments[0])
                .Invoke(this, new object[] { invocation, CancellationToken.None })!;
            return;
        }

        var context = new ClientProxyRequestContext(
            await GetActionApiDescriptionModel(invocation),
            invocation.ArgumentsDictionary,
            typeof(TService));

        if (invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
        {
            using (await InterceptorClientProxy.CallRequestAsync(context))
            {
            }
        }
        else
        {
            var resultType = invocation.Method.ReturnType.GenericTypeArguments[0];
            var result = (Task)CallRequestAsyncMethod
                .MakeGenericMethod(resultType)
                .Invoke(this, new object[] { context })!;

            invocation.ReturnValue = await GetResultAsync(result, resultType);
        }
    }

    protected virtual async Task<ActionApiDescriptionModel> GetActionApiDescriptionModel(IAbpMethodInvocation invocation)
    {
        var clientConfig = ClientOptions.HttpClientProxies.GetOrDefault(typeof(TService)) ??
                           throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");
        var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);
        var client = HttpClientFactory.Create(clientConfig.RemoteServiceName);

        return await ApiDescriptionFinder.FindActionAsync(
            client,
            remoteServiceConfig.BaseUrl,
            typeof(TService),
            invocation.Method
        );
    }

    protected virtual async Task<T> CallRequestAsync<T>(ClientProxyRequestContext context)
    {
        return await InterceptorClientProxy.CallRequestAsync<T>(context);
    }

    private static CancellationToken GetMethodCancellationToken(IAbpMethodInvocation invocation)
    {
        var parameters = invocation.Method.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(CancellationToken))
            {
                return (CancellationToken)invocation.Arguments[i]!;
            }
        }

        return default;
    }

    protected virtual async IAsyncEnumerable<T> CallRequestAsyncEnumerable<T>(IAbpMethodInvocation invocation, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var methodCancellationToken = GetMethodCancellationToken(invocation);
        using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(methodCancellationToken, cancellationToken);

        ClientProxyRequestContext context;
        using (InterceptorClientProxy.UseCancellationToken(linkedCancellationTokenSource.Token))
        {
            context = new ClientProxyRequestContext(
                await GetActionApiDescriptionModel(invocation),
                invocation.ArgumentsDictionary,
                typeof(TService));
        }

        if (methodCancellationToken.CanBeCanceled)
        {
            // Not necessarily the last argument, which is the only one checked when sending the request.
            context.CancellationToken = methodCancellationToken;
        }

        await foreach (var item in InterceptorClientProxy.CallRequestAsyncEnumerable<T>(context).WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }

    protected virtual async Task<object> GetResultAsync(Task task, Type resultType)
    {
        await task;
        var resultProperty = typeof(Task<>)
            .MakeGenericType(resultType)
            .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public);
        Check.NotNull(resultProperty, nameof(resultProperty));
        return resultProperty!.GetValue(task)!;
    }
}
