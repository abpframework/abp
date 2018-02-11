using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Permissions
{
    public class PermissionManager_Basic_Tests : PermissionTestBase
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionManager_Basic_Tests()
        {
            _permissionManager = GetRequiredService<IPermissionManager>();
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Permission_Is_Not_Defined()
        {
            await Assert.ThrowsAsync<AbpException>(async () =>
                await _permissionManager.IsGrantedAsync("UndefinedPermissionName")
            );
        }

        [Fact]
        public async Task Should_Return_False_As_Default_For_Any_Permission()
        {
            (await _permissionManager.IsGrantedAsync("MyPermission1")).ShouldBeFalse();
        }
    }
}
