using System.Net.Http;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityModel
{
    public class IdentityModelHttpClientAuthenticateContext
    {
        public HttpClient Client { get; }

        /// <summary>
        /// The identity client name configured with the <see cref="IdentityClientOptions"/>.
        /// </summary>
        public string IdentityClientName { get; }

        /// <param name="client"><see cref="HttpClient"/> object to be authenticated</param>
        /// <param name="identityClientName">The identity client name configured with the <see cref="IdentityClientOptions"/>.</param>
        public IdentityModelHttpClientAuthenticateContext(
            [NotNull] HttpClient client,
            string identityClientName = null)
        {
            Client = Check.NotNull(client, nameof(client));
            IdentityClientName = identityClientName;
        }
    }
}
