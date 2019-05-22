using System;
using System.Threading.Tasks;
using MyCompanyName.MyProjectName.Users;
using MongoDB.Driver.Linq;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Xunit;

namespace MyCompanyName.MyProjectName.MongoDB.Samples
{
    /* This is just an example test class.
     * Normally, you don't test ABP framework code
     * (like default AppUser repository IRepository<AppUser, Guid> here).
     * Only test your custom repository methods.
     */
    public class SampleRepositoryTests : MyProjectNameMongoDbTestBase
    {
        private readonly IRepository<AppUser, Guid> _appUserRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SampleRepositoryTests()
        {
            _appUserRepository = GetRequiredService<IRepository<AppUser, Guid>>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Query_AppUser()
        {
            /* Need to manually start Unit Of Work because
             * FirstOrDefaultAsync should be executed while db connection / context is available.
             */
            using (var uow = _unitOfWorkManager.Begin())
            {
                //Act
                var adminUser = await _appUserRepository
                    .GetMongoQueryable()
                    .FirstOrDefaultAsync(u => u.UserName == "admin");

                //Assert
                adminUser.ShouldNotBeNull();

                await uow.CompleteAsync();
            }
        }
    }
}
