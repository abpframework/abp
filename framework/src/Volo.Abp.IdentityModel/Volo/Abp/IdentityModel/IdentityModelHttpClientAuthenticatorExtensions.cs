using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityModel
{
    public static class IdentityModelHttpClientAuthenticatorExtensions
    {
        /// <param name="authenticator">Authenticator object</param>
        /// <param name="client"><see cref="HttpClient"/> object to be authenticated</param>
        /// <param name="identityClientName">The identity client name configured with the <see cref="IdentityClientOptions"/>.</param>
        public static Task AuthenticateAsync(
            [NotNull] this IIdentityModelHttpClientAuthenticator authenticator,
            [NotNull] HttpClient client, 
            string identityClientName = null)
        {
            Check.NotNull(authenticator, nameof(authenticator));

            return authenticator.AuthenticateAsync(
                new IdentityModelHttpClientAuthenticateContext(
                    client,
                    identityClientName
                )
            );
        }
    }
}
