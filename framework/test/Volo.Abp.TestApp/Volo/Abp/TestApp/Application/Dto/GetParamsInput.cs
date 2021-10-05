using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class GetParamsInput
    {
        public List<GetParamsNameValue> NameValues { get; set; }

        public GetParamsNameValue NameValue { get; set; }
    }

    public class GetParamsNameValue
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    [ExposeServices(typeof(IObjectToQueryString<List<GetParamsNameValue>>))]
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

    [ExposeServices(typeof(IObjectToFormData<List<GetParamsNameValue>>))]
    public class TestObjectToFormData : IObjectToFormData<List<GetParamsNameValue>>, ITransientDependency
    {
        public Task<List<KeyValuePair<string, HttpContent>>> ConvertAsync(List<GetParamsNameValue> values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            var formDataContents = new List<KeyValuePair<string, HttpContent>>();
            for (var i = 0; i < values.Count; i++)
            {
                formDataContents.Add(new KeyValuePair<string, HttpContent>($"NameValues[{i}].Name", new StringContent(values[i].Name, Encoding.UTF8)));
                formDataContents.Add(new KeyValuePair<string, HttpContent>($"NameValues[{i}].Value", new StringContent(values[i].Value, Encoding.UTF8)));
            }

            return Task.FromResult(formDataContents);
        }
    }
}
