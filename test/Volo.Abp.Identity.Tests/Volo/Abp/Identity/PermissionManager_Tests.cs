using System.Threading.Tasks;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class PermissionManager_Tests : AbpIdentityDomainTestBase
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionManager_Tests()
        {
            _permissionManager = GetRequiredService<IPermissionManager>();
        }

        public async Task Test1()
        {
            
        }
    }
}
