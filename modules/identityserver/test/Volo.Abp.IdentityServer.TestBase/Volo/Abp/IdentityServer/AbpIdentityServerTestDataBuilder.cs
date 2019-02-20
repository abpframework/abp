using System.Collections.Generic;
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
        private readonly AbpIdentityServerTestData _testData;

        public AbpIdentityServerTestDataBuilder(
            IGuidGenerator guidGenerator,
            IApiResourceRepository apiResourceRepository,
            IClientRepository clientRepository,
            IIdentityResourceRepository identityResourceRepository,
            AbpIdentityServerTestData testData
            /*IPersistentGrantRepository persistentGrantRepository*/)
        {
            _testData = testData;
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
            var identityResource = new IdentityResource(_testData.IdentityResource1Id, "NewIdentityResource1")
            {
                Description = nameof(Client.Description),
                DisplayName = nameof(IdentityResource.DisplayName)
            };

            identityResource.AddUserClaim(nameof(ApiResourceClaim.Type));

            _identityResourceRepository.Insert(identityResource);
            _identityResourceRepository.Insert(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource2"));
            _identityResourceRepository.Insert(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource3"));
        }

        private void AddApiResources()
        {
            var apiResource = new ApiResource(_testData.ApiResource1Id, "NewApiResource1");
            apiResource.Description = nameof(apiResource.Description);
            apiResource.DisplayName = nameof(apiResource.DisplayName);

            apiResource.AddScope(nameof(ApiScope.Name));
            apiResource.AddUserClaim(nameof(ApiResourceClaim.Type));
            apiResource.AddSecret(nameof(ApiSecret.Value));

            _apiResourceRepository.Insert(apiResource);
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create(), "NewApiResource2"));
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create(), "NewApiResource3"));
        }

        private void AddClients()
        {
            var client = new Client(_testData.Client1Id, "ClientId1")
            {
                Description = nameof(Client.Description),
                ClientName = nameof(Client.ClientName),
                ClientUri = nameof(Client.ClientUri),
                LogoUri = nameof(Client.LogoUri),
                ProtocolType = nameof(Client.ProtocolType),
                FrontChannelLogoutUri = nameof(Client.FrontChannelLogoutUri)
            };

            client.AddCorsOrigin("https://client1-origin.com");
            client.AddClaim(nameof(ClientClaim.Value), nameof(ClientClaim.Type));
            client.AddGrantType(nameof(ClientGrantType.GrantType));
            client.AddIdentityProviderRestriction(nameof(ClientIdPRestriction.Provider));
            client.AddPostLogoutRedirectUri(nameof(ClientPostLogoutRedirectUri.PostLogoutRedirectUri));
            client.AddProperty(nameof(ClientProperty.Key), nameof(ClientProperty.Value));
            client.AddRedirectUri(nameof(ClientRedirectUri.RedirectUri));
            client.AddScope(nameof(ClientScope.Scope));
            client.AddSecret(nameof(ClientSecret.Value));

            _clientRepository.Insert(client);

            _clientRepository.Insert(new Client(_guidGenerator.Create(), "ClientId2"));
            _clientRepository.Insert(new Client(_guidGenerator.Create(), "ClientId3"));
        }
    }
}
