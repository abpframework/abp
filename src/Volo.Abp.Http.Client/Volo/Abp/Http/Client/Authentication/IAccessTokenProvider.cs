using System.Threading.Tasks;

namespace Volo.Abp.Http.Client.Authentication
{
    public interface IAccessTokenProvider //TODO: Not sure if this class should be here
    {
        Task<string> GetOrNullAsync();
    }
}
