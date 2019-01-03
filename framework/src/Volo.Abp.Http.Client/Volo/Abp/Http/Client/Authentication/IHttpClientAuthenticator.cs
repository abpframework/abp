using System.Threading.Tasks;

namespace Volo.Abp.Http.Client.Authentication
{
    public interface IHttpClientAuthenticator
    {
        Task Authenticate(HttpClientAuthenticateContext context);
    }
}