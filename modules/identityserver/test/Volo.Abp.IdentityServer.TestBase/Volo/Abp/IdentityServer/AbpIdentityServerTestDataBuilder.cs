using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        //private readonly IPersistentGrantRepository _persistentGrantRepository;

        public AbpIdentityServerTestDataBuilder(
            IGuidGenerator guidGenerator,
            IApiResourceRepository apiResourceRepository,
            IClientRepository clientRepository,
            IIdentityResourceRepository identityResourceRepository
            /*IPersistentGrantRepository persistentGrantRepository*/)
        {
            _guidGenerator = guidGenerator;
            _apiResourceRepository = apiResourceRepository;
            _clientRepository = clientRepository;
            _identityResourceRepository = identityResourceRepository;
            //_persistentGrantRepository = persistentGrantRepository;
        }

        public void Build()
        {
            AddPersistedGrants();
            AddIdentityResources();
            AddApiResources();
            AddClients();
        }

        private void AddPersistedGrants()
        {
            //_persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create()));
            //_persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create()));
            //_persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create()));
        }

        private void AddIdentityResources()
        {
            _identityResourceRepository.Insert(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource1"));
            _identityResourceRepository.Insert(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource2"));
            _identityResourceRepository.Insert(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource3"));
        }

        private void AddApiResources()
        {
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create(), "NewApiResource1"));
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create(), "NewApiResource2"));
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create(), "NewApiResource3"));
        }

        private void AddClients()
        {
            _clientRepository.Insert(new Client(_guidGenerator.Create(), "ClientId1"));
            _clientRepository.Insert(new Client(_guidGenerator.Create(), "ClientId2"));
            _clientRepository.Insert(new Client(_guidGenerator.Create(), "ClientId3"));
        }
    }
}
