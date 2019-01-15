using System.Threading.Tasks;

namespace Volo.Abp.IdentityModel
{
    public interface IIdentityModelHttpClientAuthenticator
    {
        Task Authenticate(IdentityModelHttpClientAuthenticateContext context);
    }
}