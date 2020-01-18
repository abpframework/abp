﻿using System.Linq;
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

        protected ClientRepository_Tests()
        {
            clientRepository = ServiceProvider.GetRequiredService<IClientRepository>();
        }

        [Fact]
        public async Task FindByCliendIdAsync()
        {
            (await clientRepository.FindByCliendIdAsync("ClientId2").ConfigureAwait(false)).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllDistinctAllowedCorsOriginsAsync()
        {
            var origins = await clientRepository.GetAllDistinctAllowedCorsOriginsAsync().ConfigureAwait(false);
            origins.Any().ShouldBeTrue();
        }
    }
}
