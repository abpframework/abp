using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;
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
                var url = GenerateUrlWithParameters(actionApiDescription, invocation);

                var response = await client.GetAsync(proxyConfig.BaseUrl + url);
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

        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action, IAbpMethodInvocation invocation)
        {
            //TODO: Can be optimized using StringBuilder?
            var url = ReplacePathVariables(action.Url, action.Parameters, invocation);
            url = AddQueryStringParameters(url, action.Parameters, invocation);
            return url;
        }

        private static string ReplacePathVariables(string url, IList<ParameterApiDescriptionModel> actionParameters, IAbpMethodInvocation invocation)
        {
            var pathParameters = actionParameters
                .Where(p => p.BindingSourceId == "Path")
                .ToArray();

            if (!pathParameters.Any())
            {
                return url;
            }

            foreach (var pathParameter in pathParameters)
            {
                url = url.Replace($"{{{pathParameter.Name}}}", FindParameterValue(invocation, pathParameter));
            }

            return url;
        }

        private static string AddQueryStringParameters(string url, IList<ParameterApiDescriptionModel> actionParameters, IAbpMethodInvocation invocation)
        {
            var queryStringParameters = actionParameters
                .Where(p => p.BindingSourceId.IsIn("ModelBinding", "Query"))
                .ToArray();

            if (!queryStringParameters.Any())
            {
                return url;
            }

            var qsBuilder = new StringBuilder();

            foreach (var queryStringParameter in queryStringParameters)
            {
                var value = FindParameterValue(invocation, queryStringParameter);
                if (value == null)
                {
                    continue;
                }

                qsBuilder.Append(qsBuilder.Length == 0 ? "?" : "&");
                qsBuilder.Append(queryStringParameter.Name + "=" + value); //TODO: URL Encode!
            }

            return url + qsBuilder;
        }

        private static string FindParameterValue(IAbpMethodInvocation invocation, ParameterApiDescriptionModel parameter)
        {
            //TODO: Handle null values

            if (parameter.Name == parameter.NameOnMethod)
            {
                return invocation.ArgumentsDictionary[parameter.Name]?.ToString();
            }
            else
            {
                var obj = invocation.ArgumentsDictionary[parameter.NameOnMethod];
                if (obj == null)
                {
                    return null;
                }

                return ReflectionHelper.GetValueByPath(obj, obj.GetType(), parameter.Name)?.ToString();
            }
        }
    }
}