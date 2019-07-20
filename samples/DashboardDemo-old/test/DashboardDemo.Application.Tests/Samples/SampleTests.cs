using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DashboardDemo.Users;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Xunit;

namespace DashboardDemo.Samples
{
    public class SampleTests : DashboardDemoApplicationTestBase
    {
        private readonly IIdentityUserAppService _userAppService;
        private readonly IRepository<AppUser, Guid> _appUserRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SampleTests()
        {
            _userAppService = ServiceProvider.GetRequiredService<IIdentityUserAppService>();
            _appUserRepository = ServiceProvider.GetRequiredService<IRepository<AppUser, Guid>>();
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Admin_User()
        {
            //Act
            var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());

            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(u => u.UserName == "admin");
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
                    .Where(u => u.UserName == "admin")
                    .FirstOrDefaultAsync();

                //Assert
                adminUser.ShouldNotBeNull();

                await uow.CompleteAsync();
            }
        }
    }
}
