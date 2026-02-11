using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer;
using Volo.Abp.PermissionManagement;
using Xunit;

namespace Volo.Abp.OpenIddict;

public class PermissionManager_Tests : AbpIdentityServerDomainTestBase
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionStore _permissionStore;
    private readonly AbpIdentityServerTestData _testData;

    public PermissionManager_Tests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionStore = GetRequiredService<IPermissionStore>();
        _testData = GetRequiredService<AbpIdentityServerTestData>();
    }

    [Fact]
    public async Task Should_Grant_Permission_To_Client()
    {
        (await _permissionManager.GetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission1)).IsGranted.ShouldBeFalse();
        (await _permissionStore.IsGrantedAsync(TestPermissionNames.MyPermission2, ClientPermissionValueProvider.ProviderName, _testData.Client1Name)).ShouldBeFalse();

        await _permissionManager.SetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission2, true);

        (await _permissionManager.GetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission2)).IsGranted.ShouldBeTrue();
        (await _permissionStore.IsGrantedAsync(TestPermissionNames.MyPermission2, ClientPermissionValueProvider.ProviderName, _testData.Client1Name)).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Revoke_Permission_From_Client()
    {
        await _permissionManager.SetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission1, true);
        (await _permissionManager.GetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission1)).IsGranted.ShouldBeTrue();

        await _permissionManager.SetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission1, false);
        (await _permissionManager.GetForClientAsync(_testData.Client1Name, TestPermissionNames.MyPermission1)).IsGranted.ShouldBeFalse();
    }
}
