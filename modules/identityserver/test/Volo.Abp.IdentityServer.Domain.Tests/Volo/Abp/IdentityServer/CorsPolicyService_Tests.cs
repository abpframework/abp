using System.Threading.Tasks;
using IdentityServer4.Services;
using Shouldly;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.IdentityServer;

public class CorsPolicyService_Tests : AbpIdentityServerDomainTestBase
{
    private readonly ICorsPolicyService _corsPolicyService;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public CorsPolicyService_Tests()
    {
        _corsPolicyService = GetRequiredService<ICorsPolicyService>();
        _clientRepository = GetRequiredService<IClientRepository>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
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

        using (var uow = _unitOfWorkManager.Begin())
        {
            var client1 = await _clientRepository.FindByClientIdAsync("ClientId1");
            client1.AddCorsOrigin("https://new-origin.com");
            await _clientRepository.UpdateAsync(client1);

            await uow.CompleteAsync();
        }

        //It does exists now
        (await _corsPolicyService.IsOriginAllowedAsync("https://new-origin.com")).ShouldBeTrue();
    }
}
