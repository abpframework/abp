using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Timing;
using IdentityServer4.Models;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;
using ClientClaim = Volo.Abp.IdentityServer.Clients.ClientClaim;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;
using PersistedGrant = Volo.Abp.IdentityServer.Grants.PersistedGrant;

namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly IPersistentGrantRepository _persistentGrantRepository;
        private readonly IDeviceFlowCodesRepository _deviceFlowCodesRepository;
        private readonly AbpIdentityServerTestData _testData;
        private readonly IClock _clock;

        public AbpIdentityServerTestDataBuilder(
            IGuidGenerator guidGenerator,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IClientRepository clientRepository,
            IIdentityResourceRepository identityResourceRepository,
            IIdentityClaimTypeRepository identityClaimTypeRepository,
            AbpIdentityServerTestData testData,
            IPersistentGrantRepository persistentGrantRepository,
            IDeviceFlowCodesRepository deviceFlowCodesRepository,
            IClock clock)
        {
            _testData = testData;
            _guidGenerator = guidGenerator;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _clientRepository = clientRepository;
            _identityResourceRepository = identityResourceRepository;
            _identityClaimTypeRepository = identityClaimTypeRepository;
            _persistentGrantRepository = persistentGrantRepository;
            _deviceFlowCodesRepository = deviceFlowCodesRepository;
            _clock = clock;
        }

        public async Task BuildAsync()
        {
            await AddApiScopes();
            await AddApiResources();
            await AddIdentityResources();
            await AddClients();
            await AddPersistentGrants();
            await AddDeviceFlowCodes();
            await AddPersistedGrants();
            await AddClaimTypes();
        }

        private async Task AddApiScopes()
        {
            var apiScope = new ApiScope(_guidGenerator.Create(), "Test-ApiScope-Name-1");

            apiScope.AddUserClaim("Test-ApiScope-Claim-Type-1");
            await _apiScopeRepository.InsertAsync(apiScope);

            var apiScope2 = new ApiScope(_guidGenerator.Create(), "Test-ApiScope-Name-2");
            await _apiScopeRepository.InsertAsync(apiScope2);
        }

        private async Task AddApiResources()
        {
            var apiResource = new ApiResource(_testData.ApiResource1Id, "NewApiResource1");
            apiResource.Description = nameof(apiResource.Description);
            apiResource.DisplayName = nameof(apiResource.DisplayName);

            apiResource.AddScope(nameof(ApiResourceScope.Scope));
            apiResource.AddUserClaim(nameof(ApiResourceClaim.Type));
            apiResource.AddSecret(nameof(ApiResourceSecret.Value));

            await _apiResourceRepository.InsertAsync(apiResource);
            await _apiResourceRepository.InsertAsync(new ApiResource(_guidGenerator.Create(), "NewApiResource2"));
            await _apiResourceRepository.InsertAsync(new ApiResource(_guidGenerator.Create(), "NewApiResource3"));

            var apiResource2 = new ApiResource(_guidGenerator.Create(), "Test-ApiResource-Name-1")
            {
                Enabled = true,
                Description = "Test-ApiResource-Description-1",
                DisplayName = "Test-ApiResource-DisplayName-1"
            };

            apiResource2.AddSecret("secret".Sha256());
            apiResource2.AddScope("Test-ApiResource-ApiScope-Name-1");
            apiResource2.AddScope("Test-ApiResource-ApiScope-DisplayName-1");
            apiResource2.AddUserClaim("Test-ApiResource-Claim-Type-1");

            await _apiResourceRepository.InsertAsync(apiResource2);
        }

        private async Task AddIdentityResources()
        {
            var identityResource1 = new IdentityResource(_testData.IdentityResource1Id, "NewIdentityResource1")
            {
                Description = nameof(Client.Description),
                DisplayName = nameof(IdentityResource.DisplayName)
            };

            identityResource1.AddUserClaim(nameof(ApiResourceClaim.Type));

            await _identityResourceRepository.InsertAsync(identityResource1);
            await _identityResourceRepository.InsertAsync(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource2"));
            await _identityResourceRepository.InsertAsync(new IdentityResource(_guidGenerator.Create(), "NewIdentityResource3"));

            var identityResource2 = new IdentityResource(_guidGenerator.Create(), "Test-Identity-Resource-Name-1")
            {
                Description = "Test-Identity-Resource-Description-1",
                DisplayName = "Test-Identity-Resource-DisplayName-1",
                Required = true,
                Emphasize = true
            };

            identityResource2.AddUserClaim("Test-Identity-Resource-1-IdentityClaim-Type-1");
            await _identityResourceRepository.InsertAsync(identityResource2);
        }

          private async Task AddClients()
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
            client.AddCorsOrigin("https://{0}.abp.io");
            client.AddClaim(nameof(ClientClaim.Type), nameof(ClientClaim.Value));
            client.AddGrantType(nameof(ClientGrantType.GrantType));
            client.AddIdentityProviderRestriction(nameof(ClientIdPRestriction.Provider));
            client.AddPostLogoutRedirectUri(nameof(ClientPostLogoutRedirectUri.PostLogoutRedirectUri));
            client.AddProperty(nameof(ClientProperty.Key), nameof(ClientProperty.Value));
            client.AddRedirectUri(nameof(ClientRedirectUri.RedirectUri));
            client.AddScope(nameof(ClientScope.Scope));
            client.AddSecret(nameof(ClientSecret.Value));

            await _clientRepository.InsertAsync(client);

            await _clientRepository.InsertAsync(new Client(_guidGenerator.Create(), "ClientId2"));
            await _clientRepository.InsertAsync(new Client(_guidGenerator.Create(), "ClientId3"));


            var client42 = new Client(_guidGenerator.Create(), "42")
            {
                ProtocolType = "TestProtocol-42"
            };

            client42.AddCorsOrigin("Origin1");
            client42.AddScope("Test-ApiScope-Name-1");
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

        private async Task AddPersistedGrants()
        {
            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "PersistedGrantKey1",
                SubjectId = "PersistedGrantSubjectId1",
                SessionId = "PersistedGrantSessionId1",
                ClientId = "PersistedGrantClientId1",
                Type = "PersistedGrantType1",
                Data = ""
            });

            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "PersistedGrantKey2",
                SubjectId = "PersistedGrantSubjectId2",
                ClientId = "c1",
                Type = "c1type",
                Data = ""
            });

            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "PersistedGrantKey3",
                SubjectId = "PersistedGrantSubjectId3",
                ClientId = "c1",
                Type = "c1type",
                Data = "",
                Expiration = _clock.Now.AddDays(1),
            });

            await _persistentGrantRepository.InsertAsync(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "PersistedGrantKey_Expired1",
                SubjectId = "PersistedGrantSubjectId_Expired1",
                ClientId = "c1",
                Type = "c1type",
                Data = "",
                Expiration = _clock.Now.AddDays(-1)
            });
        }

        private async Task AddDeviceFlowCodes()
        {
            await _deviceFlowCodesRepository.InsertAsync(
                new DeviceFlowCodes(_guidGenerator.Create())
                {
                    ClientId = "c1",
                    DeviceCode = "DeviceCode1",
                    Expiration = _clock.Now.AddDays(1),
                    Data = "{\"Lifetime\":\"42\"}",
                    UserCode = "DeviceFlowCodesUserCode1",
                    SubjectId = "DeviceFlowCodesSubjectId1"
                }
            );

            await _deviceFlowCodesRepository.InsertAsync(
                new DeviceFlowCodes(_guidGenerator.Create())
                {
                    ClientId = "c1",
                    DeviceCode = "DeviceCode2",
                    Expiration = _clock.Now.AddDays(-1),
                    Data = "",
                    UserCode = "DeviceFlowCodesUserCode2",
                    SubjectId = "DeviceFlowCodesSubjectId2"
                }
            );

        }

        private async Task AddClaimTypes()
        {
            var ageClaim = new IdentityClaimType(Guid.NewGuid(), "Age", false, false, null, null, null,
                IdentityClaimValueType.Int);
            await _identityClaimTypeRepository.InsertAsync(ageClaim);
        }
    }
}
