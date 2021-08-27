using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class DynamicHttpProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
    {
        // ReSharper disable once StaticMemberInGenericType
        protected static MethodInfo MakeRequestAndGetResultAsyncMethod { get; }

        protected AbpHttpClientOptions ClientOptions { get; }
        protected IHttpProxyExecuter HttpProxyExecuter { get; }
        protected IDynamicProxyHttpClientFactory HttpClientFactory { get; }
        protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }
        protected IApiDescriptionFinder ApiDescriptionFinder { get; }

        public ILogger<DynamicHttpProxyInterceptor<TService>> Logger { get; set; }

        static DynamicHttpProxyInterceptor()
        {
            MakeRequestAndGetResultAsyncMethod = typeof(HttpProxyExecuter)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.Name == nameof(IHttpProxyExecuter.MakeRequestAndGetResultAsync) && m.IsGenericMethodDefinition);
        }

        public DynamicHttpProxyInterceptor(
            IHttpProxyExecuter httpProxyExecuter,
            IOptions<AbpHttpClientOptions> clientOptions,
            IDynamicProxyHttpClientFactory httpClientFactory,
            IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider,
            IApiDescriptionFinder apiDescriptionFinder)
        {
            HttpProxyExecuter = httpProxyExecuter;
            HttpClientFactory = httpClientFactory;
            RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
            ApiDescriptionFinder = apiDescriptionFinder;
            ClientOptions = clientOptions.Value;

            Logger = NullLogger<DynamicHttpProxyInterceptor<TService>>.Instance;
        }


        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var context = new HttpProxyExecuterContext(
                await GetActionApiDescriptionModel(invocation),
                invocation.ArgumentsDictionary,
                typeof(TService));

            if (invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
            {
                await HttpProxyExecuter.MakeRequestAsync(context);
            }
            else
            {
                var result = (Task)MakeRequestAndGetResultAsyncMethod
                    .MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments[0])
                    .Invoke(HttpProxyExecuter, new object[] { context });

                invocation.ReturnValue = await GetResultAsync(
                    result,
                    invocation.Method.ReturnType.GetGenericArguments()[0]
                );
            }
        }

        private async Task<ActionApiDescriptionModel> GetActionApiDescriptionModel(IAbpMethodInvocation invocation)
        {
            var clientConfig = ClientOptions.HttpClientProxies.GetOrDefault(typeof(TService)) ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);
            var client = HttpClientFactory.Create(clientConfig.RemoteServiceName);

            return await ApiDescriptionFinder.FindActionAsync(
                client,
                remoteServiceConfig.BaseUrl,
                typeof(TService),
                invocation.Method
            );
        }

        private async Task<object> GetResultAsync(Task task, Type resultType)
        {
            await task;
            return typeof(Task<>)
                .MakeGenericType(resultType)
                .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public)
                .GetValue(task);
        }
    }
}
