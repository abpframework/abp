using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.DynamicProxying
{
    public class DynamicHttpProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
    {
        private static readonly MethodInfo GenericInterceptAsyncMethod;

        private readonly IDynamicProxyHttpClientFactory _httpClientFactory;
        private readonly IApplicationApiDescriptionModelManager _discoverManager;
        private readonly AbpHttpOptions _options;

        static DynamicHttpProxyInterceptor()
        {
            GenericInterceptAsyncMethod = typeof(DynamicHttpProxyInterceptor<TService>)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(InterceptAsync) && m.IsGenericMethodDefinition);
        }

        public DynamicHttpProxyInterceptor(
            IDynamicProxyHttpClientFactory httpClientFactory, 
            IOptions<AbpHttpOptions> options,
            IApplicationApiDescriptionModelManager discoverManager)
        {
            _httpClientFactory = httpClientFactory;
            _discoverManager = discoverManager;
            _options = options.Value;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            AsyncHelper.RunSync(() => InterceptAsync(invocation));
        }

        public override Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            invocation.ReturnValue = GenericInterceptAsyncMethod
                .MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments[0])
                .Invoke(this, new object[]{ invocation });

            return Task.CompletedTask;
        }

        private async Task<T> InterceptAsync<T>(IAbpMethodInvocation invocation)
        {
            var config = _options.HttpClientProxies.GetOrDefault(typeof(TService));
            if (config == null)
            {
                throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(T).FullName}.");
            }

            var apiDescriptionModel = await _discoverManager.GetAsync(config.BaseUrl);

            
            using (var client = _httpClientFactory.Create())
            {
                var response = await client.GetAsync("/api/app/people");
                if (!response.IsSuccessStatusCode)
                {
                    throw new AbpException("Remote service returns error!");
                }

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject(
                    content,
                    typeof(T),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                return (T)result;
            }
        }
    }
}