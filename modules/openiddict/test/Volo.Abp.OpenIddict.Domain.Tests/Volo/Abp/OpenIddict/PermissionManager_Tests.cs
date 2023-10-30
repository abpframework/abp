using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;
using Xunit;

namespace Volo.Abp.OpenIddict;

public class PermissionManager_Tests : OpenIddictDomainTestBase
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionStore _permissionStore;
    private readonly AbpOpenIddictTestData _testData;

    public PermissionManager_Tests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionStore = GetRequiredService<IPermissionStore>();
        _testData = GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task Should_Grant_Permission_To_Client()
    {
        (await _permissionManager.GetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission1)).IsGranted.ShouldBeFalse();
        (await _permissionStore.IsGrantedAsync(TestPermissionNames.MyPermission2, ClientPermissionValueProvider.ProviderName, _testData.App1ClientId)).ShouldBeFalse();

        await _permissionManager.SetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission2, true);

        (await _permissionManager.GetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission2)).IsGranted.ShouldBeTrue();
        (await _permissionStore.IsGrantedAsync(TestPermissionNames.MyPermission2, ClientPermissionValueProvider.ProviderName, _testData.App1ClientId)).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Revoke_Permission_From_Client()
    {
        await _permissionManager.SetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission1, true);
        (await _permissionManager.GetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission1)).IsGranted.ShouldBeTrue();

        await _permissionManager.SetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission1, false);
        (await _permissionManager.GetForClientAsync(_testData.App1ClientId, TestPermissionNames.MyPermission1)).IsGranted.ShouldBeFalse();
    }
}
