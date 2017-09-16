using System.Collections.Generic;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    internal static class HttpActionParameterHelper
    {
        public static object FindParameterValue(IReadOnlyDictionary<string, object> methodArguments, ParameterApiDescriptionModel apiParameter)
        {
            var value = methodArguments[apiParameter.NameOnMethod];
            if (value == null)
            {
                return null;
            }

            if (apiParameter.Name == apiParameter.NameOnMethod)
            {
                return value;
            }

            return ReflectionHelper.GetValueByPath(value, value.GetType(), apiParameter.Name);
        }
    }
}
