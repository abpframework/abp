using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TenantManagement
{
    public class TenantManager_Tests : AbpTenantManagementDomainTestBase
    {
        private readonly ITenantManager _tenantManager;
        private readonly ITenantRepository _tenantRepository;

        public TenantManager_Tests()
        {
            _tenantManager = GetRequiredService<ITenantManager>();
            _tenantRepository = GetRequiredService<ITenantRepository>();
        }


        [Fact]
        public async Task CreateAsync()
        {
            var tenant = await _tenantManager.CreateAsync("Test");
            tenant.Name.ShouldBe("Test");
        }

        [Fact]
        public async Task Create_Tenant_Name_Can_Not_Duplicate()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () => await _tenantManager.CreateAsync("volosoft"));
        }

        [Fact]
        public async Task ChangeNameAsync()
        {
            var tenant = await _tenantRepository.FindByNameAsync("volosoft");
            tenant.ShouldNotBeNull();

            await _tenantManager.ChangeNameAsync(tenant, "newVolosoft");

            tenant.Name.ShouldBe("newVolosoft");
        }

        [Fact]
        public async Task ChangeName_Tenant_Name_Can_Not_Duplicate()
        {
            var tenant = await _tenantRepository.FindByNameAsync("acme");
            tenant.ShouldNotBeNull();

            await Assert.ThrowsAsync<UserFriendlyException>(async () => await _tenantManager.ChangeNameAsync(tenant, "volosoft"));
        }
    }
}
