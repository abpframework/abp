using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionChecker_Basic_Tests : PermissionTestBase
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionChecker_Basic_Tests()
        {
            _permissionChecker = GetRequiredService<IPermissionChecker>();
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Permission_Is_Not_Defined()
        {
            await Assert.ThrowsAsync<AbpException>(async () =>
                await _permissionChecker.IsGrantedAsync("UndefinedPermissionName")
            );
        }

        [Fact]
        public async Task Should_Return_False_As_Default_For_Any_Permission()
        {
            (await _permissionChecker.IsGrantedAsync("MyPermission1")).ShouldBeFalse();
        }
    }
}
