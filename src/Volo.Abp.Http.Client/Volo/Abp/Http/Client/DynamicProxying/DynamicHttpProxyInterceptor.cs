using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class DynamicHttpProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
    {
        private static MethodInfo GenericInterceptAsyncMethod { get; }

        private readonly IDynamicProxyHttpClientFactory _httpClientFactory;
        private readonly IApiDescriptionFinder _apiDescriptionFinder;
        private readonly AbpHttpClientOptions _options;

        static DynamicHttpProxyInterceptor()
        {
            GenericInterceptAsyncMethod = typeof(DynamicHttpProxyInterceptor<TService>)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(InterceptAsync) && m.IsGenericMethodDefinition);
        }

        public DynamicHttpProxyInterceptor(
            IDynamicProxyHttpClientFactory httpClientFactory,
            IOptions<AbpHttpClientOptions> options,
            IApiDescriptionFinder apiDescriptionFinder)
        {
            _httpClientFactory = httpClientFactory;
            _apiDescriptionFinder = apiDescriptionFinder;
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
                .Invoke(this, new object[] { invocation });

            return Task.CompletedTask;
        }

        private async Task<T> InterceptAsync<T>(IAbpMethodInvocation invocation)
        {
            var proxyConfig = GetProxyConfig();
            var actionApiDescription = await _apiDescriptionFinder.FindActionAsync(proxyConfig, invocation.Method);

            using (var client = _httpClientFactory.Create())
            {
                var response = await client.GetAsync(proxyConfig.BaseUrl + actionApiDescription.Url);
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

        private DynamicHttpClientProxyConfig GetProxyConfig()
        {
            return _options.HttpClientProxies.GetOrDefault(typeof(TService))
                   ?? throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");
        }
    }
}