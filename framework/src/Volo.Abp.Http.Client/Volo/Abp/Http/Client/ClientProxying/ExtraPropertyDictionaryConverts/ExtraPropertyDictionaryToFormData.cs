using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.ClientProxying.ExtraPropertyDictionaryConverts;

public class ExtraPropertyDictionaryToFormData : IObjectToFormData<ExtraPropertyDictionary>, ITransientDependency
{
    public Task<List<KeyValuePair<string, HttpContent>>> ConvertAsync(ActionApiDescriptionModel actionApiDescription, ParameterApiDescriptionModel parameterApiDescription, ExtraPropertyDictionary extraPropertyDictionary)
    {
        if (extraPropertyDictionary.IsNullOrEmpty())
        {
            return Task.FromResult<List<KeyValuePair<string, HttpContent>>>(null!);
        }

        var formDataContents = new List<KeyValuePair<string, HttpContent>>();
        foreach (var item in extraPropertyDictionary)
        {
            formDataContents.Add(new KeyValuePair<string, HttpContent>($"ExtraProperties[{item.Key}]", new StringContent(item.Value!.ToString()!, Encoding.UTF8)));
        }

        return Task.FromResult(formDataContents);
    }
}
