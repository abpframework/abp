using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Clients;

public class ClientStore_Tests : AbpIdentityServerTestBase
{
    private readonly IClientStore _clientStore;

    public ClientStore_Tests()
    {
        _clientStore = ServiceProvider.GetRequiredService<IClientStore>();
    }

    [Fact]
    public async Task FindClientByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var client = await _clientStore.FindClientByIdAsync("non-existing-id");
        client.ShouldBeNull();
    }

    [Fact]
    public async Task FindClientByIdAsync_Should_Return_The_Client_If_Found()
    {
        //Act
        var client = await _clientStore.FindClientByIdAsync("42");

        //Assert
        client.ShouldNotBeNull();
        client.ClientId.ShouldBe("42");
        client.ProtocolType.ShouldBe("TestProtocol-42");
        client.AllowedCorsOrigins.ShouldContain("Origin1");
        client.AllowedScopes.ShouldContain("Test-ApiScope-Name-1");
    }
}
