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
        private readonly IClientRepository _clientRepository;
        private readonly IPersistentGrantRepository _persistentGrantRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IApiResourceRepository _apiResourceRepository;

        public AbpIdentityServerTestDataBuilder(
            IClientRepository clientRepository,
            IGuidGenerator guidGenerator,
            IPersistentGrantRepository persistentGrantRepository,
            IIdentityResourceRepository identityResourceRepository,
            IApiResourceRepository apiResourceRepository)
        {
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
            _persistentGrantRepository = persistentGrantRepository;
            _identityResourceRepository = identityResourceRepository;
            _apiResourceRepository = apiResourceRepository;
        }

        public void Build()
        {
            AddClients();
            AddPersistentGrants();
            AddApiResources();
            AddIdentityResources();
        }

        private void AddClients()
        {
            var client42 = new Client(_guidGenerator.Create())
            {
                ClientId = "42",
                ProtocolType = "TestProtocol-42"
            };

            client42.AllowedCorsOrigins.Add(
                new ClientCorsOrigin(_guidGenerator.Create())
                {
                    Origin = "Origin1"
                }
            );

            _clientRepository.Insert(client42);
        }

        private void AddPersistentGrants()
        {
            _persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "38",
                ClientId = "TestClientId-38",
                Type = "TestType-38",
                SubjectId = "TestSubject",
                Data = "TestData-38"
            });

            _persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "37",
                ClientId = "TestClientId-37",
                Type = "TestType-37",
                SubjectId = "TestSubject",
                Data = "TestData-37"
            });

            _persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "36",
                ClientId = "TestClientId-X",
                Type = "TestType-36",
                SubjectId = "TestSubject-X",
                Data = "TestData-36"
            });

            _persistentGrantRepository.Insert(new PersistedGrant(_guidGenerator.Create())
            {
                Key = "35",
                ClientId = "TestClientId-X",
                Type = "TestType-35",
                SubjectId = "TestSubject-X",
                Data = "TestData-35"
            });
        }

        private void AddApiResources()
        {
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create())
            {
                Name = "Test-ApiResource-Name-1",
                Enabled = true,
                Description = "Test-ApiResource-Description-1",
                DisplayName = "Test-ApiResource-DisplayName-1",
                Secrets = new List<ApiSecret>
                {
                    new ApiSecret(_guidGenerator.Create())
                },
                UserClaims = new List<ApiResourceClaim>
                {
                    new ApiResourceClaim(_guidGenerator.Create())
                    {
                        Type = "Test-ApiResource-Claim-Type-1"
                    }
                },
                Scopes = new List<ApiScope>
                {
                    new ApiScope(_guidGenerator.Create())
                    {
                        Name = "Test-ApiResource-ApiScope-Name-1",
                        DisplayName = "Test-ApiResource-ApiScope-DisplayName-1"
                    }
                }
            });
        }

        private void AddIdentityResources()
        {
            _identityResourceRepository.Insert(new IdentityResource(_guidGenerator.Create())
            {
                Enabled = true,
                Description = "Test-Identity-Resource-Description-1",
                DisplayName = "Test-Identity-Resource-DisplayName-1",
                Name = "Test-Identity-Resource-Name-1",
                Required = true,
                ShowInDiscoveryDocument = true,
                Emphasize = true,
                UserClaims = new List<IdentityClaim>
                {
                    new IdentityClaim(_guidGenerator.Create())
                    {
                        Type = "Test-Identity-Resource-1-IdentityClaim-Type-1"
                    }
                }
            });
        }
    }
}
