using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.Clients;

namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerTestDataBuilder : ITransientDependency
    {
        private readonly IClientRepository _clientRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AbpIdentityServerTestDataBuilder(
            IClientRepository clientRepository, 
            IGuidGenerator guidGenerator)
        {
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
        }

        public void Build()
        {
            AddClients();
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
    }
}
