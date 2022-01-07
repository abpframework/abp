using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityModel;

//TODO: Re-consider this interface!
public interface IIdentityModelAuthenticationService
{
    Task<bool> TryAuthenticateAsync(
        [NotNull] HttpClient client,
        string identityClientName = null);

    Task<string> GetAccessTokenAsync(
        IdentityClientConfiguration configuration);
}
