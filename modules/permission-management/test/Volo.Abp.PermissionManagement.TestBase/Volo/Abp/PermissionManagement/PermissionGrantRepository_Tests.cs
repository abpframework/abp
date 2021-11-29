using System.Threading.Tasks;
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
            (await PermissionGrantRepository.FindAsync("MyPermission1", UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString())).ShouldNotBeNull();

            (await PermissionGrantRepository.FindAsync("Undefined-Permission", UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString())).ShouldBeNull();
            (await PermissionGrantRepository.FindAsync("MyPermission1", "Undefined-Provider", "Unknown-Id")).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var permissionGrants = await PermissionGrantRepository.GetListAsync(UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString());

            permissionGrants.ShouldContain(p => p.Name == "MyPermission1");
        }

        [Fact]
        public async Task GetList_With_Names()
        {
            var permissionGrants = await PermissionGrantRepository.GetListAsync(new []{"MyPermission1", "MyPermission3"},UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString());

            permissionGrants.ShouldContain(p => p.Name == "MyPermission1");
            permissionGrants.ShouldContain(p => p.Name == "MyPermission3");
        }
    }
}
