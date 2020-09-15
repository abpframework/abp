﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    internal static class UrlBuilder
    {
        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion, IObjectToQueryStringConverter objectToQueryStringConverter)
        {
            var urlBuilder = new StringBuilder(action.Url);

            ReplacePathVariables(urlBuilder, action.Parameters, methodArguments, apiVersion);
            AddQueryStringParameters(urlBuilder, action.Parameters, methodArguments, apiVersion, objectToQueryStringConverter);

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

        private static void AddQueryStringParameters(StringBuilder urlBuilder, IList<ParameterApiDescriptionModel> actionParameters, IReadOnlyDictionary<string, object> methodArguments, ApiVersionInfo apiVersion, IObjectToQueryStringConverter objectToQueryStringConverter)
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

                AddQueryStringParameter(urlBuilder, isFirstParam, queryStringParameter.Name, value, objectToQueryStringConverter);

                isFirstParam = false;
            }

            if (apiVersion.ShouldSendInQueryString())
            {
                AddQueryStringParameter(urlBuilder, isFirstParam, "api-version", apiVersion.Version, objectToQueryStringConverter);  //TODO: Constant!
            }
        }

        private static void AddQueryStringParameter(
            StringBuilder urlBuilder,
            bool isFirstParam,
            string name,
            [NotNull] object value, 
            IObjectToQueryStringConverter objectToQueryStringConverter)
        {
            urlBuilder.Append(isFirstParam ? "?" : "&");
            urlBuilder.Append(objectToQueryStringConverter.ConvertObjectToQueryString(name, value));
        }
    }
}
