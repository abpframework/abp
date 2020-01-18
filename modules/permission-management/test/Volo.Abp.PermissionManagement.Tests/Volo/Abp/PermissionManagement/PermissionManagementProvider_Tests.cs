using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionManagementProvider_Tests : PermissionTestBase
    {
        private readonly IPermissionManagementProvider _permissionManagementProvider;
        private readonly IPermissionGrantRepository _permissionGrantRepository;


        public PermissionManagementProvider_Tests()
        {
            _permissionManagementProvider = GetRequiredService<TestPermissionManagementProvider>();
            _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        }


        [Fact]
        public async Task CheckAsync()
        {
            await _permissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    Guid.NewGuid(),
                    "MyPermission1",
                    "Test",
                    "Test"
                )
            ).ConfigureAwait(false);

            var permissionValueProviderGrantInfo = await _permissionManagementProvider.CheckAsync("MyPermission1",
                "Test",
                "Test").ConfigureAwait(false);

            permissionValueProviderGrantInfo.IsGranted.ShouldBeTrue();
            permissionValueProviderGrantInfo.ProviderKey.ShouldBe("Test");
        }

        [Fact]
        public async Task Check_Should_Return_NonGranted_When_ProviderName_NotEquals_Name()
        {
            var permissionValueProviderGrantInfo = await _permissionManagementProvider.CheckAsync("MyPermission1",
                "TestNotExist",
                "Test").ConfigureAwait(false);

            permissionValueProviderGrantInfo.IsGranted.ShouldBeFalse();
            permissionValueProviderGrantInfo.ProviderKey.ShouldBeNull();
        }


        [Fact]
        public async Task SetAsync()
        {
            await _permissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    Guid.NewGuid(),
                    "MyPermission1",
                    "Test",
                    "Test"
                )
            ).ConfigureAwait(false);
            (await _permissionGrantRepository.FindAsync("MyPermission1",
                "Test",
                "Test").ConfigureAwait(false)).ShouldNotBeNull();

            await _permissionManagementProvider.SetAsync("MyPermission1",
                "Test",
                false).ConfigureAwait(false);

            (await _permissionGrantRepository.FindAsync("MyPermission1",
                "Test",
                "Test").ConfigureAwait(false)).ShouldBeNull();
        }

    }
}
