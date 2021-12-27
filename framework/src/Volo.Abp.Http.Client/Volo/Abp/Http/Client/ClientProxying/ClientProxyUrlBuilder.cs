using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Proxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Localization;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyUrlBuilder : ITransientDependency
    {
        protected static MethodInfo CallObjectToQueryStringAsyncMethod { get; }

        protected static MethodInfo CallObjectToPathAsyncMethod { get; }

        static ClientProxyUrlBuilder()
        {
            CallObjectToQueryStringAsyncMethod = typeof(ClientProxyUrlBuilder)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(ObjectToQueryStringAsync) && m.IsGenericMethodDefinition);

            CallObjectToPathAsyncMethod = typeof(ClientProxyUrlBuilder)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(ObjectToPathAsync) && m.IsGenericMethodDefinition);
        }

        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected AbpHttpClientProxyingOptions HttpClientProxyingOptions { get; }

        public ClientProxyUrlBuilder(IServiceScopeFactory serviceScopeFactory, IOptions<AbpHttpClientProxyingOptions> httpClientProxyingOptions)
        {
            ServiceScopeFactory = serviceScopeFactory;
            HttpClientProxyingOptions = httpClientProxyingOptions.Value;
        }

        public async Task<string> GenerateUrlWithParametersAsync(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            // The ASP.NET Core route value provider and query string value provider:
            //  Treat values as invariant culture.
            //  Expect that URLs are culture-invariant.
            using (CultureHelper.Use(CultureInfo.InvariantCulture))
            {
                var urlBuilder = new StringBuilder(action.Url);

                await ReplacePathVariablesAsync(urlBuilder, action, methodArguments, apiVersion);
                await AddQueryStringParametersAsync(urlBuilder, action, methodArguments, apiVersion);

                return urlBuilder.ToString();
            }
        }

        protected virtual async Task ReplacePathVariablesAsync(StringBuilder urlBuilder, ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var pathParameters = action.Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Path)
                .ToArray();

            if (!pathParameters.Any())
            {
                return;
            }

            if (pathParameters.Any(p => p.Name == "apiVersion"))
            {
                urlBuilder = urlBuilder.Replace("{apiVersion}", apiVersion.Version);
            }

            foreach (var pathParameter in pathParameters.Where(p => p.Name != "apiVersion")) //TODO: Constant!
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, pathParameter);

                if (value == null)
                {
                    if (pathParameter.IsOptional)
                    {
                        urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", "");
                    }
                    else if (pathParameter.DefaultValue != null)
                    {
                        urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", await ConvertValueToStringAsync(pathParameter.DefaultValue));
                    }
                    else
                    {
                        throw new AbpException($"Missing path parameter value for {pathParameter.Name} ({pathParameter.NameOnMethod})");
                    }
                }
                else
                {
                    if (HttpClientProxyingOptions.PathConverts.ContainsKey(value.GetType()))
                    {
                        using (var scope = ServiceScopeFactory.CreateScope())
                        {
                            var path = await (Task<string>)CallObjectToPathAsyncMethod
                                .MakeGenericMethod(value.GetType())
                                .Invoke(this, new object[]
                                {
                                    scope.ServiceProvider.GetRequiredService(HttpClientProxyingOptions.PathConverts[value.GetType()]),
                                    action,
                                    pathParameter,
                                    value
                                });

                            if (path != null)
                            {
                                urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", path);
                                continue;
                            }
                        }
                    }

                    urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", await ConvertValueToStringAsync(value));
                }
            }
        }

        protected virtual async Task AddQueryStringParametersAsync(StringBuilder urlBuilder, ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var queryStringParameters = action.Parameters
                .Where(p => p.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
                .ToArray();

            var isFirstParam = true;

            foreach (var queryStringParameter in queryStringParameters)
            {
                var value = HttpActionParameterHelper.FindParameterValue(methodArguments, queryStringParameter);
                if (value == null)
                {
                    continue;
                }

                if (HttpClientProxyingOptions.QueryStringConverts.ContainsKey(value.GetType()))
                {
                    using (var scope = ServiceScopeFactory.CreateScope())
                    {
                        var queryString = await (Task<string>)CallObjectToQueryStringAsyncMethod
                            .MakeGenericMethod(value.GetType())
                            .Invoke(this, new object[]
                            {
                                scope.ServiceProvider.GetRequiredService(HttpClientProxyingOptions.QueryStringConverts[value.GetType()]),
                                action,
                                queryStringParameter,
                                value
                            });

                        if (queryString != null)
                        {
                            urlBuilder.Append(isFirstParam ? "?" : "&");
                            urlBuilder.Append(queryString);
                            isFirstParam = false;
                            continue;
                        }
                    }
                }

                if (await AddQueryStringParameterAsync(urlBuilder, isFirstParam, queryStringParameter.Name, value))
                {
                    isFirstParam = false;
                }
            }

            if (apiVersion.ShouldSendInQueryString())
            {
                await AddQueryStringParameterAsync(urlBuilder, isFirstParam, "api-version", apiVersion.Version);  //TODO: Constant!
            }
        }

        protected virtual async Task<string> ObjectToQueryStringAsync<T>(IObjectToQueryString<T> converter, ActionApiDescriptionModel actionApiDescription, ParameterApiDescriptionModel parameterApiDescription, T value)
        {
            return await converter.ConvertAsync(actionApiDescription, parameterApiDescription, value);
        }

        protected virtual async Task<string> ObjectToPathAsync<T>(IObjectToPath<T> converter, ActionApiDescriptionModel actionApiDescription, ParameterApiDescriptionModel parameterApiDescription, T value)
        {
            return await converter.ConvertAsync(actionApiDescription, parameterApiDescription, value);
        }

        protected virtual async Task<bool> AddQueryStringParameterAsync(
            StringBuilder urlBuilder,
            bool isFirstParam,
            string name,
            [NotNull] object value)
        {
            if (value.GetType().IsArray || (value.GetType().IsGenericType && value is IEnumerable))
            {
                var index = 0;
                foreach (var item in (IEnumerable) value)
                {
                    if (index == 0)
                    {
                        urlBuilder.Append(isFirstParam ? "?" : "&");
                    }
                    urlBuilder.Append(name + $"[{index++}]=" + System.Net.WebUtility.UrlEncode(await ConvertValueToStringAsync(item)) + "&");
                }

                if (index > 0)
                {
                    //remove & at the end of the urlBuilder.
                    urlBuilder.Remove(urlBuilder.Length - 1, 1);
                    return true;
                }

                return false;
            }

            urlBuilder.Append(isFirstParam ? "?" : "&");
            urlBuilder.Append(name + "=" + System.Net.WebUtility.UrlEncode(await ConvertValueToStringAsync(value)));
            return true;
        }

        protected virtual Task<string> ConvertValueToStringAsync([CanBeNull] object value)
        {
            if (value is DateTime dateTimeValue)
            {
                return Task.FromResult(dateTimeValue.ToUniversalTime().ToString("O"));
            }

            return Task.FromResult(value?.ToString());
        }
    }
}
