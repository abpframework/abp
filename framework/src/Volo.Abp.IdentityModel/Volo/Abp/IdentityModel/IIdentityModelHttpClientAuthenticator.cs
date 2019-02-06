using System.Threading.Tasks;

namespace Volo.Abp.IdentityModel
{
    public interface IIdentityModelHttpClientAuthenticator
    {
        Task AuthenticateAsync(IdentityModelHttpClientAuthenticateContext context);
    }
}