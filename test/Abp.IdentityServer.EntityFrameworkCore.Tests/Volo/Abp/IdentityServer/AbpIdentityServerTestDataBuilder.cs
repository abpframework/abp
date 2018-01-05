using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;

namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerTestDataBuilder : ITransientDependency
    {
        private readonly IClientRepository _clientRepository;
        private readonly IPersistentGrantRepository _persistentGrantRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AbpIdentityServerTestDataBuilder(
            IClientRepository clientRepository, 
            IGuidGenerator guidGenerator, 
            IPersistentGrantRepository persistentGrantRepository)
        {
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
            _persistentGrantRepository = persistentGrantRepository;
        }

        public void Build()
        {
            AddClients();
            AddPersistentGrants();
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
            var persistentGrant38 = new PersistedGrant(_guidGenerator.Create())
            {
                Key = "38",
                ClientId = "TestClientId-38",
                Type = "TestType-38",
                SubjectId = "TestSubject-38",
                Data = "TestData-38"
            };

            _persistentGrantRepository.Insert(persistentGrant38);
        }
    }
}
