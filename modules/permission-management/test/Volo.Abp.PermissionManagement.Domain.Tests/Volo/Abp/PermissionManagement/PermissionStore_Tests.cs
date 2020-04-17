using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement
{
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
    }
}
