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
        private readonly IApplicationApiDescriptionModelManager _discoverManager;
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
                .Invoke(this, new object[] { invocation });

            return Task.CompletedTask;
        }

        private async Task<T> InterceptAsync<T>(IAbpMethodInvocation invocation)
        {
            var proxyConfig = GetProxyConfig();
            var apiDescription = await _discoverManager.GetAsync(proxyConfig.BaseUrl);
            var actionApiDescription = FindAction(apiDescription, invocation.Method, proxyConfig);

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
            var config = _options.HttpClientProxies.GetOrDefault(typeof(TService));
            if (config == null)
            {
                throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {typeof(TService).FullName}.");
            }
            return config;
        }

        private static ActionApiDescriptionModel FindAction(ApplicationApiDescriptionModel apiDescriptionModel, MethodInfo method, DynamicHttpClientProxyConfig config)
        {
            //TODO: Move to another class and cache results!

            var methodParameters = method.GetParameters().ToArray();

            foreach (var module in apiDescriptionModel.Modules.Values)
            {
                if (module.Name != config.ModuleName)
                {
                    continue;
                }

                foreach (var controller in module.Controllers.Values)
                {
                    if (controller.Interfaces.All(i => i.TypeAsString != typeof(TService).FullName))
                    {
                        continue;
                    }

                    foreach (var action in controller.Actions.Values)
                    {
                        if (action.NameOnClass == method.Name && action.ParametersOnMethod.Count == methodParameters.Length)
                        {
                            var found = true;

                            for (int i = 0; i < methodParameters.Length; i++)
                            {
                                if (action.ParametersOnMethod[i].TypeAsString != methodParameters[i].ParameterType.FullName)
                                {
                                    found = false;
                                    break;
                                }
                            }

                            if (found)
                            {
                                return action;
                            }
                        }
                    }
                }
            }

            throw new AbpException("Could not found remote action for method: " + method);
        }
    }
}