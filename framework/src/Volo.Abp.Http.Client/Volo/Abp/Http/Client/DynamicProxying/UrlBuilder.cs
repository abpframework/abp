using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Localization;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    internal static class UrlBuilder
    {
        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var urlBuilder = new StringBuilder(action.Url);

            ReplacePathVariables(urlBuilder, action.Parameters, methodArguments, apiVersion);
            AddQueryStringParameters(urlBuilder, action.Parameters, methodArguments, apiVersion);

            return urlBuilder.ToString();
        }

        private static void ReplacePathVariables(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var pathParameters = actionParameters
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
                        urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", pathParameter.DefaultValue.ToString());
                    }
                    else
                    {
                        throw new AbpException($"Missing path parameter value for {pathParameter.Name} ({pathParameter.NameOnMethod})");
                    }
                }
                else
                {
                    urlBuilder = urlBuilder.Replace($"{{{pathParameter.Name}}}", value.ToString());
                }
            }
        }

        private static void AddQueryStringParameters(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion)
        {
            var queryStringParameters = actionParameters
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

                AddQueryStringParameter(urlBuilder, isFirstParam, queryStringParameter.Name, value);

                isFirstParam = false;
            }

            if (apiVersion.ShouldSendInQueryString())
            {
                AddQueryStringParameter(urlBuilder, isFirstParam, "api-version", apiVersion.Version);  //TODO: Constant!
            }
        }

        private static void AddQueryStringParameter(
            StringBuilder urlBuilder,
            bool isFirstParam,
            string name,
            [NotNull] object value)
        {
            urlBuilder.Append(isFirstParam ? "?" : "&");

            (string, string) GetKeyAndValue(string keyValuePair)
            {
                var pairString = keyValuePair.ToString().Trim('[', ']').Split(',');
                return (pairString[0], pairString[1].IsNullOrWhiteSpace() ? null : pairString[1].Trim());
            }

            if (value.GetType().IsArray || (value.GetType().IsGenericType && value is IEnumerable))
            {
                var index = 0;
                var parameters = new List<string>();

                foreach (var item in (IEnumerable)value)
                {
                    if (item != null && ReflectionHelper.IsAssignableToGenericType(item.GetType(), typeof(KeyValuePair<,>)))
                    {
                        var keyValuePair = GetKeyAndValue(item.ToString());
                        var encodedKey = System.Net.WebUtility.UrlEncode(ConvertValueToString(keyValuePair.Item1));
                        parameters.Add(name + $"[{encodedKey}]=" + System.Net.WebUtility.UrlEncode(ConvertValueToString(keyValuePair.Item2)));
                    }
                    else
                    {
                        parameters.Add(name + $"[{index++}]=" + System.Net.WebUtility.UrlEncode(ConvertValueToString(item)));
                    }
                }

                urlBuilder.Append(string.Join("&", parameters));
            }
            else
            {
                urlBuilder.Append(name + "=" + System.Net.WebUtility.UrlEncode(ConvertValueToString(value)));
            }
        }

        private static string ConvertValueToString([CanBeNull] object value)
        {
            if (value == null) return null;

            using (CultureHelper.Use(CultureInfo.InvariantCulture))
            {
                if (value is DateTime dateTimeValue)
                {
                    return dateTimeValue.ToUniversalTime().ToString("u");
                }

                return value.ToString();
            }
        }
    }
}
