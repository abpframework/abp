using System.Threading.Tasks;
using IdentityServer4.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;
using PersistedGrant = Volo.Abp.IdentityServer.Grants.PersistedGrant;

namespace Volo.Abp.IdentityServer
{
    //TODO: There are two data builders (see  AbpIdentityServerTestDataBuilder in Volo.Abp.IdentityServer.TestBase). It should be somehow unified!

    public class AbpIdentityServerTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClientRepository _clientRepository;
        private readonly IPersistentGrantRepository _persistentGrantRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public AbpIdentityServerTestDataBuilder(
            IClientRepository clientRepository,
            IGuidGenerator guidGenerator,
            IPersistentGrantRepository persistentGrantRepository,
            IApiResourceRepository apiResourceRepository, 
            IIdentityResourceRepository identityResourceRepository)
        {
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
            _persistentGrantRepository = persistentGrantRepository;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceRepository = identityResourceRepository;
        }

        public async Task BuildAsync()
        {
            await AddClients();
            await AddPersistentGrants();
            await AddApiResources();
            await AddIdentityResources();
        }

        private async Task AddClients()
        {
            var client42 = new Client(_guidGenerator.Create(), "42")
            {
                ProtocolType = "TestProtocol-42"
            };
            
            client42.AddCorsOrigin("Origin1");

            client42.AddScope("api1");

            await _clientRepository.InsertAsync(client42);
        }

        private async Task AddPersistentGrants()
        {
            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "38",
                ClientId = "TestClientId-38",
                Type = "TestType-38",
                SubjectId = "TestSubject",
                Data = "TestData-38"
            });

            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "37",
                ClientId = "TestClientId-37",
                Type = "TestType-37",
                SubjectId = "TestSubject",
                Data = "TestData-37"
            });

            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "36",
                ClientId = "TestClientId-X",
                Type = "TestType-36",
                SubjectId = "TestSubject-X",
                Data = "TestData-36"
            });

            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "35",
                ClientId = "TestClientId-X",
                Type = "TestType-35",
                SubjectId = "TestSubject-X",
                Data = "TestData-35"
            });
        }

        private async Task AddApiResources()
        {
            var apiResource = new ApiResource(_guidGenerator.Create(), "Test-ApiResource-Name-1")
            {
                Enabled = true,
                Description = "Test-ApiResource-Description-1",
                DisplayName = "Test-ApiResource-DisplayName-1"
            };

            apiResource.AddSecret("secret".Sha256());
            apiResource.AddScope("Test-ApiResource-ApiScope-Name-1", "Test-ApiResource-ApiScope-DisplayName-1");
            apiResource.AddUserClaim("Test-ApiResource-Claim-Type-1");

            await _apiResourceRepository.InsertAsync(apiResource);
        }

        private async Task AddIdentityResources()
        {
            var identityResource = new IdentityResource(_guidGenerator.Create(), "Test-Identity-Resource-Name-1")
            {
                Description = "Test-Identity-Resource-Description-1",
                DisplayName = "Test-Identity-Resource-DisplayName-1",
                Required = true,
                Emphasize = true
            };

            identityResource.AddUserClaim("Test-Identity-Resource-1-IdentityClaim-Type-1");

            await _identityResourceRepository.InsertAsync(identityResource);
        }
    }
}
