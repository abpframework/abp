using System.Threading.Tasks;

namespace Volo.Abp.Http
{
    public interface IObjectToQueryString<in TValue>
    {
        Task<string> ConvertAsync(TValue value);
    }
}
