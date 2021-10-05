using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Volo.Abp.Http
{
    public interface IObjectToFormData<in TValue>
    {
        Task<List<KeyValuePair<string, HttpContent>>> ConvertAsync(TValue value);
    }
}
