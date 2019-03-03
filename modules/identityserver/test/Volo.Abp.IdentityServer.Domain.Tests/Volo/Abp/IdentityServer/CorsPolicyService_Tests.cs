using System.Threading.Tasks;
using IdentityServer4.Services;
using Shouldly;
using Volo.Abp.IdentityServer.Clients;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public class CorsPolicyService_Tests : AbpIdentityServerDomainTestBase
    {
        private readonly ICorsPolicyService _corsPolicyService;
        private readonly IClientRepository _clientRepository;

        public CorsPolicyService_Tests()
        {
            _corsPolicyService = GetRequiredService<ICorsPolicyService>();
            _clientRepository = GetRequiredService<IClientRepository>();
        }

        [Fact]
        public async Task IsOriginAllowedAsync()
        {
            (await _corsPolicyService.IsOriginAllowedAsync("https://client1-origin.com")).ShouldBeTrue();
            (await _corsPolicyService.IsOriginAllowedAsync("https://unknown-origin.com")).ShouldBeFalse();
        }

        [Fact]
        public async Task IsOriginAllowedAsync_Should_Invalidate_Cache_On_Update()
        {
            //It does not exists before
            (await _corsPolicyService.IsOriginAllowedAsync("https://new-origin.com")).ShouldBeFalse();

            var client1 = await _clientRepository.FindByCliendIdAsync("ClientId1");
            client1.AddCorsOrigin("https://new-origin.com");
            await _clientRepository.UpdateAsync(client1);

            //It does exists now
            (await _corsPolicyService.IsOriginAllowedAsync("https://new-origin.com")).ShouldBeTrue();
        }
    }
}
