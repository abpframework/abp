using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Devices;

public class DeviceFlowStore_Tests : AbpIdentityServerTestBase
{
    private readonly IDeviceFlowStore _deviceFlowStore;

    public DeviceFlowStore_Tests()
    {
        _deviceFlowStore = GetRequiredService<IDeviceFlowStore>();
    }

    [Fact]
    public async Task StoreDeviceAuthorizationAsync()
    {
        await _deviceFlowStore.StoreDeviceAuthorizationAsync(
            "DeviceCode-Test1",
            "UserCode-Test1",
            new DeviceCode
            {
                ClientId = "ClientId1",
                AuthorizedScopes = new string[] { "s1", "s2" },
                IsAuthorized = true,
                Lifetime = 42,
                RequestedScopes = new string[] { "rs1" },
                Subject = new ClaimsPrincipal(
                    new[]
                    {
                            new ClaimsIdentity(new[]
                            {
                                new Claim(JwtClaimTypes.Subject, "sid1")
                            })
                    }
                )
            }
        );

        void Check(DeviceCode deviceCode)
        {
            deviceCode.ClientId.ShouldBe("ClientId1");
            deviceCode.AuthorizedScopes.ShouldNotBeNull();
            deviceCode.AuthorizedScopes.Count().ShouldBe(2);
            deviceCode.AuthorizedScopes.ShouldContain("s1");
            deviceCode.AuthorizedScopes.ShouldContain("s2");
            deviceCode.IsAuthorized.ShouldBeTrue();
            deviceCode.Lifetime.ShouldBe(42);
            deviceCode.RequestedScopes.ShouldNotBeNull();
            deviceCode.RequestedScopes.Count().ShouldBe(1);
            deviceCode.RequestedScopes.ShouldContain("rs1");
            deviceCode.Subject.ShouldNotBeNull();
            deviceCode.Subject.Claims.ShouldContain(x => x.Type == JwtClaimTypes.Subject && x.Value == "sid1");
        }

        Check(await _deviceFlowStore.FindByUserCodeAsync("UserCode-Test1"));
        Check(await _deviceFlowStore.FindByDeviceCodeAsync("DeviceCode-Test1"));
    }

    [Fact]
    public async Task RemoveByDeviceCodeAsync()
    {
        (await _deviceFlowStore.FindByDeviceCodeAsync("DeviceCode1")).ShouldNotBeNull();

        await _deviceFlowStore.RemoveByDeviceCodeAsync("DeviceCode1");

        (await _deviceFlowStore.FindByDeviceCodeAsync("DeviceCode1")).ShouldBeNull();
    }

    [Fact]
    public async Task UpdateByDeviceCodeAsync()
    {
        var deviceCode = await _deviceFlowStore.FindByDeviceCodeAsync("DeviceCode1");
        deviceCode.ShouldNotBeNull();
        deviceCode.Lifetime.ShouldBe(42);

        await _deviceFlowStore.UpdateByUserCodeAsync(
            "DeviceFlowCodesUserCode1",
            new DeviceCode
            {
                Lifetime = 43
            }
        );

        deviceCode = await _deviceFlowStore.FindByDeviceCodeAsync("DeviceCode1");
        deviceCode.Lifetime.ShouldBe(43);
    }
}
