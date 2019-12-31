using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.UI;
using Xunit;

namespace Volo.Abp.TenantManagement
{
    public class TenantAppService_Tests : AbpTenantManagementApplicationTestBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantAppService_Tests()
        {
            _tenantAppService = GetRequiredService<ITenantAppService>();
        }

        [Fact]
        public async Task GetAsync()
        {
            var tenantInDb = UsingDbContext(dbContext => dbContext.Tenants.First());
            var tenant = await _tenantAppService.GetAsync(tenantInDb.Id).ConfigureAwait(false);
            tenant.Name.ShouldBe(tenantInDb.Name);
        }

        [Fact]
        public async Task GetListAsync()
        {
            var result = await _tenantAppService.GetListAsync(new GetTenantsInput()).ConfigureAwait(false);
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(t => t.Name == "acme");
            result.Items.ShouldContain(t => t.Name == "volosoft");
        }

        [Fact]
        public async Task GetListAsync_Filtered()
        {
            var result = await _tenantAppService.GetListAsync(new GetTenantsInput { Filter = "volo" }).ConfigureAwait(false);
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldNotContain(t => t.Name == "acme");
            result.Items.ShouldContain(t => t.Name == "volosoft");
        }

        [Fact]
        public async Task GetListAsync_Sorted_Descending_By_Name()
        {
            var result = await _tenantAppService.GetListAsync(new GetTenantsInput { Sorting = "Name DESC" }).ConfigureAwait(false);
            result.TotalCount.ShouldBeGreaterThan(0);
            var tenants = result.Items.ToList();

            tenants.ShouldContain(t => t.Name == "acme");
            tenants.ShouldContain(t => t.Name == "volosoft");

            tenants.FindIndex(t => t.Name == "acme").ShouldBeGreaterThan(tenants.FindIndex(t => t.Name == "volosoft"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            var tenancyName = Guid.NewGuid().ToString("N").ToLowerInvariant();
            var tenant = await _tenantAppService.CreateAsync(new TenantCreateDto { Name = tenancyName }).ConfigureAwait(false);
            tenant.Name.ShouldBe(tenancyName);
            tenant.Id.ShouldNotBe(default(Guid));
        }

        [Fact]
        public async Task CreateAsync_Should_Not_Allow_Duplicate_Names()
        {
            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _tenantAppService.CreateAsync(new TenantCreateDto { Name = "acme" }).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var acme = UsingDbContext(dbContext => dbContext.Tenants.Single(t => t.Name == "acme"));

            var result = await _tenantAppService.UpdateAsync(acme.Id, new TenantUpdateDto { Name = "acme-renamed" }).ConfigureAwait(false);
            result.Id.ShouldBe(acme.Id);
            result.Name.ShouldBe("acme-renamed");

            var acmeUpdated = UsingDbContext(dbContext => dbContext.Tenants.Single(t => t.Id == acme.Id));
            acmeUpdated.Name.ShouldBe("acme-renamed");
        }

        [Fact]
        public async Task UpdateAsync_Should_Not_Allow_Duplicate_Names()
        {
            var acme = UsingDbContext(dbContext => dbContext.Tenants.Single(t => t.Name == "acme"));

            await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            {
                await _tenantAppService.UpdateAsync(acme.Id, new TenantUpdateDto { Name = "volosoft" }).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var acme = UsingDbContext(dbContext => dbContext.Tenants.Single(t => t.Name == "acme"));

            await _tenantAppService.DeleteAsync(acme.Id).ConfigureAwait(false);

            UsingDbContext(dbContext =>
            {
                dbContext.Tenants.Any(t => t.Id == acme.Id).ShouldBeFalse();
            });
        }
    }
}
