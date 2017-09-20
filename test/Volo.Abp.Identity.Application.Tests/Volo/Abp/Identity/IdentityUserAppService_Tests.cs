using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityUserAppService_Tests : AbpIdentityApplicationTestBase
    {
        private readonly IIdentityUserAppService _userAppService;
        private readonly IIdentityUserRepository _userRepository;

        public IdentityUserAppService_Tests()
        {
            _userAppService = ServiceProvider.GetRequiredService<IIdentityUserAppService>();
            _userRepository = ServiceProvider.GetRequiredService<IIdentityUserRepository>();
        }

        [Fact]
        public async Task GetAsync()
        {
            //Arrange

            var johnNash = await GetUserAsync("john.nash");

            //Act

            var result = await _userAppService.GetAsync(johnNash.Id);

            //Assert

            result.Id.ShouldBe(johnNash.Id);
            result.UserName.ShouldBe(johnNash.UserName);
            result.Email.ShouldBe(johnNash.Email);
            result.LockoutEnabled.ShouldBe(johnNash.LockoutEnabled);
            result.PhoneNumber.ShouldBe(johnNash.PhoneNumber);
        }

        [Fact]
        public async Task GetListAsync()
        {
            //Act

            var result = await _userAppService.GetListAsync(new PagedAndSortedResultRequestDto());

            //Assert

            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CreateAsync()
        {
            //Arrange

            var input = new IdentityUserCreateDto
            {
                UserName = Guid.NewGuid().ToString(),
                Email = CreateRandomEmail(),
                LockoutEnabled = true,
                PhoneNumber = CreateRandomPhoneNumber(),
                Password = "123qwe",
                RoleNames = new[] { "moderator" }
            };

            //Act

            var result = await _userAppService.CreateAsync(input);

            //Assert

            result.Id.ShouldNotBe(Guid.Empty);
            result.UserName.ShouldBe(input.UserName);
            result.Email.ShouldBe(input.Email);
            result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            result.PhoneNumber.ShouldBe(input.PhoneNumber);

            var user = await _userRepository.GetAsync(result.Id);
            user.Id.ShouldBe(result.Id);
            user.UserName.ShouldBe(input.UserName);
            user.Email.ShouldBe(input.Email);
            user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            user.PhoneNumber.ShouldBe(input.PhoneNumber);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            //Arrange

            var johnNash = await GetUserAsync("john.nash");

            var input = new IdentityUserUpdateDto
            {
                UserName = johnNash.UserName,
                LockoutEnabled = true,
                TwoFactorEnabled = true,
                PhoneNumber = CreateRandomPhoneNumber(),
                Email = CreateRandomEmail()
            };

            //Act

            var result = await _userAppService.UpdateAsync(johnNash.Id, input);

            //Assert

            result.Id.ShouldBe(johnNash.Id);
            result.UserName.ShouldBe(input.UserName);
            result.Email.ShouldBe(input.Email);
            result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            result.PhoneNumber.ShouldBe(input.PhoneNumber);

            var user = await _userRepository.GetAsync(result.Id);
            user.Id.ShouldBe(result.Id);
            user.UserName.ShouldBe(input.UserName);
            user.Email.ShouldBe(input.Email);
            user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            user.PhoneNumber.ShouldBe(input.PhoneNumber);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            //Arrange

            var johnNash = await GetUserAsync("john.nash");

            //Act

            await _userAppService.DeleteAsync(johnNash.Id);

            //Assert

            (await FindUserAsync("john.nash")).ShouldBeNull();
        }

        [Fact]
        public async Task GetRolesAsync()
        {
            //Arrange

            var johnNash = await GetUserAsync("john.nash");

            //Act

            var result = await _userAppService.GetRolesAsync(johnNash.Id);

            //Assert

            result.Items.Count.ShouldBe(2);
            result.Items.ShouldContain(r => r.Name == "moderator");
            result.Items.ShouldContain(r => r.Name == "supporter");
        }

        [Fact]
        public async Task UpdateRolesAsync()
        {
            //Arrange

            var johnNash = await GetUserAsync("john.nash");

            //Act

            await _userAppService.UpdateRolesAsync(
                johnNash.Id,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = new[] {"moderator", "admin"}
                }
            );

            //Assert

            var roleNames = await _userRepository.GetRoleNamesAsync(johnNash.Id);
            roleNames.Count.ShouldBe(2);
            roleNames.ShouldContain("admin");
            roleNames.ShouldContain("moderator");
        }

        private async Task<IdentityUser> GetUserAsync(string userName)
        {
            return (await _userRepository.GetListAsync()).First(u => u.UserName == userName);
        }

        private async Task<IdentityUser> FindUserAsync(string userName)
        {
            return (await _userRepository.GetListAsync()).FirstOrDefault(u => u.UserName == userName);
        }

        private static string CreateRandomEmail()
        {
            return Guid.NewGuid().ToString("N").Left(16) + "@abp.io";
        }

        private static string CreateRandomPhoneNumber()
        {
            return RandomHelper.GetRandom(10000000, 100000000).ToString();
        }
    }
}
