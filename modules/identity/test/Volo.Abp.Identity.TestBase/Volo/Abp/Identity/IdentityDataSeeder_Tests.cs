using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class IdentityDataSeeder_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly ICurrentTenant _currentTenant;

        protected IdentityDataSeeder_Tests()
        {
            _identityDataSeeder = GetRequiredService<IIdentityDataSeeder>();
            _userRepository = GetRequiredService<IIdentityUserRepository>();
            _roleRepository = GetRequiredService<IIdentityRoleRepository>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _currentTenant = GetRequiredService<ICurrentTenant>();
        }

        [Fact]
        public async Task Should_Create_Admin_User_And_Role()
        {
            await _identityDataSeeder.SeedAsync("admin@abp.io", "1q2w3E*");

            (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
            (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).Name.ShouldBe("admin");
            (await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Create_Admin_User_And_Role_With_TenantId()
        {
            var tenantId = Guid.NewGuid();

            await _identityDataSeeder.SeedAsync("admin@tenant.abp.io", "1q2w3E*", tenantId);

            (await _userRepository.FindByNormalizedEmailAsync(_lookupNormalizer.NormalizeEmail("admin@tenant.abp.io"))).ShouldBeNull();

            using (_currentTenant.Change(tenantId))
            {
                (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
                (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).Name.ShouldBe("admin");
                (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).TenantId.ShouldBe(tenantId);

                (await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
                (await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"))).TenantId.ShouldBe(tenantId);
            }
        }
    }
}
