using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Modularity;
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

        protected IdentityDataSeeder_Tests()
        {
            _identityDataSeeder = GetRequiredService<IIdentityDataSeeder>();
            _userRepository = GetRequiredService<IIdentityUserRepository>();
            _roleRepository = GetRequiredService<IIdentityRoleRepository>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
        }

        [Fact]
        public async Task Should_Create_Admin_User_And_Role()
        {
            await _identityDataSeeder.SeedAsync("admin@abp.io", "1q2w3E*");

            (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
            (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"))).Name.ShouldBe("admin");
            (await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
        }
    }
}
