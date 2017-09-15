using System;
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
            //url = AddQueryStringParameters(url, action.Parameters);
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
                url = url.Replace($"{{{pathParameter.Name}}}", FindParameterValue(invocation.Method, invocation.Arguments, pathParameter.Name));
            }

            return url;
        }

        //private static string AddQueryStringParameters(string url, IList<ParameterApiDescriptionModel> actionParameters)
        //{
        //    var queryStringParameters = actionParameters
        //        .Where(p => p.BindingSourceId.IsIn("ModelBinding", "Query"))
        //        .ToArray();

        //    if (!queryStringParameters.Any())
        //    {
        //        return url;
        //    }

        //    var qsBuilderParams = queryStringParameters
        //        .Select(p => $"{{ name: '{p.Name.ToCamelCase()}', value: {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(p)} }}")
        //        .JoinAsString(", ");

        //    return url + $"' + abp.utils.buildQueryString([{qsBuilderParams}]) + '";
        //}

        private static string FindParameterValue(MethodInfo method, object[] arguments, string parameterName)
        {
            var methodParameters = method.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].Name == parameterName)
                {
                    return arguments[i].ToString();
                }
            }

            throw new AbpException("Could not find parameter in the invocation: " + parameterName);
        }
    }
}