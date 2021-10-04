using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class GetParamsFromQueryInput
    {
        public List<GetParamsFromQueryInputNameValue> NameValues { get; set; }

        public GetParamsFromQueryInputNameValue NameValue { get; set; }
    }

    public class GetParamsFromQueryInputNameValue
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    [ExposeServices(typeof(IObjectToQueryString<List<GetParamsFromQueryInputNameValue>>))]
    public class TestInputToQueryString : IObjectToQueryString<List<GetParamsFromQueryInputNameValue>>, ITransientDependency
    {
        public Task<string> ConvertAsync(List<GetParamsFromQueryInputNameValue> values)
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
