using System.Threading.Tasks;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public interface IObjectToQueryString<in TValue>
    {
        Task<string> ConvertAsync(TValue value);
    }
}
