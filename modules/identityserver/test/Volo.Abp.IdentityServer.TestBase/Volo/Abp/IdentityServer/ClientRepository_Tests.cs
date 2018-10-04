using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public abstract class ClientRepository_Tests<TStartupModule> : AbpIdentityServerTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IClientRepository clientRepository { get; }

        public ClientRepository_Tests()
        {
            clientRepository = ServiceProvider.GetRequiredService<IClientRepository>();
        }

        [Fact]
        public async Task FindByCliendIdAsync()
        {
            (await clientRepository.FindByCliendIdAsync("ClientId2")).ShouldNotBeNull();
        }
    }
}
