using System.Collections.Generic;
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

        public void Build()
        {
            AddClients();
            AddPersistentGrants();
            AddApiResources();
            AddIdentityResources();
        }

        private void AddClients()
        {
            var client42 = new Client(_guidGenerator.Create(), "42")
            {
                ProtocolType = "TestProtocol-42"
            };

            client42.AllowedCorsOrigins.Add(
                new ClientCorsOrigin(_guidGenerator.Create())
                {
                    Origin = "Origin1",
                    ClientId = client42.Id
                }
            );

            client42.AddAllowedScope(_guidGenerator, "api1");

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
            _apiResourceRepository.Insert(new ApiResource(_guidGenerator.Create(), "Test-ApiResource-Name-1")
            {
                Enabled = true,
                Description = "Test-ApiResource-Description-1",
                DisplayName = "Test-ApiResource-DisplayName-1",
                Secrets = 
                {
                    new ApiSecret(_guidGenerator.Create(), "secret".Sha256())
                },
                UserClaims =
                {
                    new ApiResourceClaim(_guidGenerator.Create())
                    {
                        Type = "Test-ApiResource-Claim-Type-1"
                    }
                },
                Scopes =
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
