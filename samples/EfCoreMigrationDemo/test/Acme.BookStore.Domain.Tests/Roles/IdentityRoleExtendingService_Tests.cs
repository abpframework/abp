using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Identity;
using Xunit;

namespace Acme.BookStore.Roles
{
    public class IdentityRoleExtendingService_Tests : BookStoreDomainTestBase
    {
        private readonly IdentityRoleExtendingService _identityRoleExtendingService;
        private readonly IIdentityRoleRepository _identityRoleRepository;

        public IdentityRoleExtendingService_Tests()
        {
            _identityRoleExtendingService = GetRequiredService<IdentityRoleExtendingService>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
        }

        [Fact]
        public async Task Should_Set_And_Get_ExtraProperties()
        {
            var adminRole = await _identityRoleRepository.FindByNormalizedNameAsync("ADMIN");

            await _identityRoleExtendingService.SetTitleAsync(adminRole.Id, "New Title!");

            (await _identityRoleExtendingService.GetTitleAsync(adminRole.Id)).ShouldBe("New Title!");
        }
    }
}
