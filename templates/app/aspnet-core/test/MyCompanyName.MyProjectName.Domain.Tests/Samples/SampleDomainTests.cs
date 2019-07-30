using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Identity;
using Xunit;

namespace MyCompanyName.MyProjectName.Samples
{
    /* This is just an example test class.
     * Normally, you don't test code of the modules you are using
     * (like IdentityUserManager here).
     * Only test your own domain services.
     */
    public class SampleDomainTests : MyProjectNameDomainTestBase
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IdentityUserManager _identityUserManager;

        public SampleDomainTests()
        {
            _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
            _identityUserManager = GetRequiredService<IdentityUserManager>();
        }

        [Fact]
        public async Task Should_Set_Email_Of_A_User()
        {
            IdentityUser adminUser;

            /* Need to manually start Unit Of Work because
             * FirstOrDefaultAsync should be executed while db connection / context is available.
             */
            await WithUnitOfWorkAsync(async () =>
            {
                adminUser = await _identityUserRepository
                    .FindByNormalizedUserNameAsync("ADMIN");

                await _identityUserManager.SetEmailAsync(adminUser, "newemail@abp.io");
                await _identityUserRepository.UpdateAsync(adminUser);
            });

            adminUser = await _identityUserRepository.FindByNormalizedUserNameAsync("ADMIN");
            adminUser.Email.ShouldBe("newemail@abp.io");
        }
    }
}
