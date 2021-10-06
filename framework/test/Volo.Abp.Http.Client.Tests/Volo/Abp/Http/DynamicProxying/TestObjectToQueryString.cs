using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.Http.DynamicProxying
{
    public class TestObjectToQueryString : IObjectToQueryString<List<GetParamsNameValue>>, ITransientDependency
    {
        public Task<string> ConvertAsync(List<GetParamsNameValue> values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < values.Count; i++)
            {
                sb.Append($"NameValues[{i}].Name={values[i].Name}&NameValues[{i}].Value={values[i].Value}&");
            }

            sb.Remove(sb.Length - 1, 1);
            return Task.FromResult(sb.ToString());
        }
    }
}
