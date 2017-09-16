using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public static class RequestPayloadBuilder
    {
        public static string GenerateBody(ActionApiDescriptionModel action, IReadOnlyDictionary<string, object> methodArguments, IJsonSerializer jsonSerializer)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == "Body")
                .ToArray();

            if (parameters.Length <= 0)
            {
                return null;
            }

            if (parameters.Length > 1)
            {
                throw new AbpException(
                    $"Only one complex type allowed as argument to a controller action that's binding source is 'Body'. But action on URL: {action.Url} contains more than one!"
                );
            }

            var value = HttpActionParameterHelper.FindParameterValue(methodArguments, parameters[0]);
            if (value == null)
            {
                return null;
            }

            return jsonSerializer.Serialize(value);
        }
    }
}
