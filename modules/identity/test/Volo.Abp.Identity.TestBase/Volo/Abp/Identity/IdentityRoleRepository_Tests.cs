using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class IdentityRoleRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }

        protected IdentityRoleRepository_Tests()
        {
            RoleRepository = ServiceProvider.GetRequiredService<IIdentityRoleRepository>();
            LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
        }

        [Fact]
        public async Task FindByNormalizedNameAsync()
        {
            (await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("admin")).ConfigureAwait(false)).ShouldNotBeNull();
            (await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("undefined-role")).ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var roles = await RoleRepository.GetListAsync().ConfigureAwait(false);
            roles.ShouldContain(r => r.Name == "admin");
            roles.ShouldContain(r => r.Name == "moderator");
            roles.ShouldContain(r => r.Name == "supporter");
        }

        [Fact]
        public async Task GetCountAsync()
        {
            (await RoleRepository.GetCountAsync().ConfigureAwait(false)).ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Eager_Load_Role_Collections()
        {
            var role = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("moderator")).ConfigureAwait(false);
            role.Claims.ShouldNotBeNull();
            role.Claims.Any().ShouldBeTrue();
        }
    }
}
