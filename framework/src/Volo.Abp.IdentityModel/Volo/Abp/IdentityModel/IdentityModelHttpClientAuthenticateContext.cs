using System.Net.Http;

namespace Volo.Abp.IdentityModel
{
    public class IdentityModelHttpClientAuthenticateContext
    {
        public HttpClient Client { get; }

        /// <summary>
        /// The identity client name configured with the <see cref="IdentityClientOptions"/>.
        /// </summary>
        public string IdentityClientName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/> object to be authorized</param>
        /// <param name="identityClientName">The identity client name configured with the <see cref="IdentityClientOptions"/>.</param>
        public IdentityModelHttpClientAuthenticateContext(
            HttpClient client,
            string identityClientName = null)
        {
            Client = client;
            IdentityClientName = identityClientName;
        }
    }
}
