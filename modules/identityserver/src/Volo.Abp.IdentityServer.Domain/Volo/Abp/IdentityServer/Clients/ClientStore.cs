using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientStore : IClientStore
    {
        protected IClientRepository ClientRepository { get; }
        protected IObjectMapper<AbpIdentityServerDomainModule> ObjectMapper { get; }
        protected IDistributedCache<IdentityServer4.Models.Client> Cache { get; }
        protected IdentityServerOptions Options { get; }

        public ClientStore(
            IClientRepository clientRepository,
            IObjectMapper<AbpIdentityServerDomainModule> objectMapper,
            IDistributedCache<IdentityServer4.Models.Client> cache,
            IOptions<IdentityServerOptions> options)
        {
            ClientRepository = clientRepository;
            ObjectMapper = objectMapper;
            Cache = cache;
            Options = options.Value;
        }

        public virtual async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            return await GetCacheItemAsync(clientId);
        }

        protected virtual async Task<IdentityServer4.Models.Client> GetCacheItemAsync(string clientId)
        {
            return await Cache.GetOrAddAsync(clientId, async () =>
                {
                    var client = await ClientRepository.FindByClientIdAsync(clientId);
                    return ObjectMapper.Map<Client, IdentityServer4.Models.Client>(client);
                },
                optionsFactory: () => new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
                },
                considerUow: true);

        }
    }
}
