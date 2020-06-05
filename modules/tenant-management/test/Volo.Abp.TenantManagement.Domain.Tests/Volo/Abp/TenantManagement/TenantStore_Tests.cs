using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.TenantManagement
{
    public class TenantStore_Tests : AbpTenantManagementDomainTestBase
    {
        private readonly ITenantStore _tenantStore;
        private readonly ITenantRepository _tenantRepository;

        public TenantStore_Tests()
        {
            _tenantStore = GetRequiredService<ITenantStore>();
            _tenantRepository = GetRequiredService<ITenantRepository>();
        }

        [Fact]
        public async Task FindAsyncByName()
        {
            var acme = await _tenantStore.FindAsync("acme");
            acme.ShouldNotBeNull();
            acme.Name.ShouldBe("acme");
        }

        [Fact]
        public async Task FindAsyncById()
        {
            var acme = await _tenantRepository.FindByNameAsync("acme");
            acme.ShouldNotBeNull();

             (await _tenantStore.FindAsync(acme.Id)).ShouldNotBeNull();
        }
    }
}
