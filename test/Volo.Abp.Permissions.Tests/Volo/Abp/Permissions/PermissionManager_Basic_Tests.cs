using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Permissions
{
    public class PermissionManager_Basic_Tests : AbpPermissionTestBase
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionManager_Basic_Tests()
        {
            _permissionManager = GetRequiredService<IPermissionManager>();
        }

        [Fact]
        public async Task Test1()
        {
            _permissionManager.ShouldNotBeNull();
        }
    }
}
