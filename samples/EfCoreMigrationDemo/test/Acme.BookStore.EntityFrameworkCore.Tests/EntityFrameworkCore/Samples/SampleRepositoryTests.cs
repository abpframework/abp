using Microsoft.EntityFrameworkCore;
using Acme.BookStore.Users;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Roles;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Acme.BookStore.EntityFrameworkCore.Samples
{
    /* This is just an example test class.
     * Normally, you don't test ABP framework code
     * (like default AppUser repository IRepository<AppUser, Guid> here).
     * Only test your custom repository methods.
     */
    public class SampleRepositoryTests : BookStoreEntityFrameworkCoreTestBase
    {
        private readonly IRepository<AppUser, Guid> _appUserRepository;
        private readonly IRepository<AppRole, Guid> _appRoleRepository;

        public SampleRepositoryTests()
        {
            _appUserRepository = GetRequiredService<IRepository<AppUser, Guid>>();
            _appRoleRepository = GetRequiredService<IRepository<AppRole, Guid>>();
        }

        [Fact]
        public async Task Should_Query_AppUser()
        {
            /* Need to manually start Unit Of Work because
             * FirstOrDefaultAsync should be executed while db connection / context is available.
             */
            await WithUnitOfWorkAsync(async () =>
            {
                //Act
                var adminUser = await _appUserRepository
                    .Where(u => u.UserName == "admin")
                    .FirstOrDefaultAsync();

                //Assert
                adminUser.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task Should_Query_And_Update_AppRole()
        {
            /* Need to manually start Unit Of Work because
             * FirstOrDefaultAsync should be executed while db connection / context is available.
             */
            await WithUnitOfWorkAsync(async () =>
            {
                //Act
                var adminRole = await _appRoleRepository
                    .Where(u => u.Name == "admin")
                    .FirstOrDefaultAsync();

                //Assert
                adminRole.ShouldNotBeNull();
                adminRole.Title.ShouldBeNull();

                //Act
                adminRole.Title = "Admin's Fancy Title!";
                await _appRoleRepository.UpdateAsync(adminRole);
            });

            await WithUnitOfWorkAsync(async () =>
            {
                //Act
                var adminRole = await _appRoleRepository
                    .Where(u => u.Name == "admin")
                    .FirstOrDefaultAsync();

                //Assert
                adminRole.ShouldNotBeNull();
                adminRole.Title.ShouldBe("Admin's Fancy Title!");
            });
        }
    }
}
