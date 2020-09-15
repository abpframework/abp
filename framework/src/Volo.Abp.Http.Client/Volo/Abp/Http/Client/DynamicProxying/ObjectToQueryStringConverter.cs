using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class ObjectToQueryStringConverter : IObjectToQueryStringConverter, ITransientDependency
    {
        public string ConvertObjectToQueryString(string name, object obj)
        {
            return SerializeToQueryString(obj, name);
        }

        private string SerializeToQueryString(object obj, string prefix)
        {
            var queryStrings = new List<string>();
            var type = obj.GetType();

            if (TypeHelper.IsPrimitiveExtended(type))
            {
                queryStrings.Add($"{prefix}={System.Net.WebUtility.UrlEncode(ConvertValueToString(obj))}");
            }
            else if (obj is IDictionary dict && type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
            {
                foreach (DictionaryEntry kv in dict)
                {
                    var value = kv.Value;
                    if (value == null) continue;
                    string name = $"{prefix}.{kv.Key}";
                    AddQueryString(name, value, queryStrings);
                }
            }
            else if (type.IsArray || type.IsGenericType && obj is IEnumerable)
            {
                var index = 0;
                foreach (var value in (IEnumerable) obj)
                {
                    if (value == null) continue;
                    string name = $"{prefix}[{index++}]";
                    AddQueryString(name, value, queryStrings);
                }
            }
            else
            {
                var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var prop in props)
                {
                    var value = prop.GetValue(obj);
                    if (value == null) continue;
                    string name = $"{prefix}.{prop.Name}";
                    AddQueryString(name, value, queryStrings);
                }
            }

            return String.Join("&", queryStrings);
        }

        private void AddQueryString(string name, object value, ICollection<string> queryStrings)
        {
            string queryString = TypeHelper.IsPrimitiveExtended(value.GetType()) ? $"{name}={System.Net.WebUtility.UrlEncode(ConvertValueToString(value))}" : SerializeToQueryString(value, name);
            queryStrings.Add(queryString);
        }

        private static string ConvertValueToString([NotNull] object value)
        {
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