﻿using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.PermissionManagement
{
    public abstract class PermissionGrantRepository_Tests<TStartupModule> : PermissionManagementTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected PermissionGrantRepository_Tests()
        {
            PermissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        }

        [Fact]
        public async Task FindAsync()
        {
            (await PermissionGrantRepository.FindAsync("MyPermission1", UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString()).ConfigureAwait(false)).ShouldNotBeNull();

            (await PermissionGrantRepository.FindAsync("Undefined-Permission", UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString()).ConfigureAwait(false)).ShouldBeNull();
            (await PermissionGrantRepository.FindAsync("MyPermission1", "Undefined-Provider", "Unknown-Id").ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var permissionGrants = await PermissionGrantRepository.GetListAsync(UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString()).ConfigureAwait(false);

            permissionGrants.ShouldContain(p => p.Name == "MyPermission1");
        }
    }
}
