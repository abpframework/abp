using System.Threading.Tasks;
using IdentityServer4.Stores;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientStore : IClientStore
    {
        private readonly IClientRepository _clientRepository;
        private readonly IObjectMapper<AbpIdentityServerDomainModule> _objectMapper;

        public ClientStore(IClientRepository clientRepository, IObjectMapper<AbpIdentityServerDomainModule> objectMapper)
        {
            _clientRepository = clientRepository;
            _objectMapper = objectMapper;
        }

        public virtual async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            var client = await _clientRepository.FindByCliendIdAsync(clientId);
            return _objectMapper.Map<Client, IdentityServer4.Models.Client>(client);
        }
    }
}
