using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionStore_Tests : PermissionTestBase
{
    private readonly IPermissionStore _permissionStore;

    public PermissionStore_Tests()
    {
        _permissionStore = GetRequiredService<IPermissionStore>();
    }

    [Fact]
    public async Task IsGrantedAsync()
    {
        (await _permissionStore.IsGrantedAsync("MyPermission1",
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString())).ShouldBeTrue();

        (await _permissionStore.IsGrantedAsync("MyPermission1NotExist",
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString())).ShouldBeFalse();
    }

    [Fact]
    public async Task IsGranted_Multiple()
    {
        var result = await _permissionStore.IsGrantedAsync(new[] { "MyPermission1", "MyPermission1NotExist" },
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        result.Result.Count.ShouldBe(2);

        result.Result.FirstOrDefault(x => x.Key == "MyPermission1").Value.ShouldBe(PermissionGrantResult.Granted);
        result.Result.FirstOrDefault(x => x.Key == "MyPermission1NotExist").Value.ShouldBe(PermissionGrantResult.Undefined);
    }
}
