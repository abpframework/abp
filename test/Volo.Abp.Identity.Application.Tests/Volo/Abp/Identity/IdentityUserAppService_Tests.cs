using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityUserAppService_Tests : AbpIdentityApplicationTestBase
    {
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IRepository<IdentityUser> _userRepository;

        public IdentityUserAppService_Tests()
        {
            _identityUserAppService = ServiceProvider.GetRequiredService<IIdentityUserAppService>();
            _userRepository = ServiceProvider.GetRequiredService<IRepository<IdentityUser>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var result = await _identityUserAppService.GetListAsync(new PagedAndSortedResultRequestDto());
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
                Password = "123qwe"
            };

            //Act

            var result = await _identityUserAppService.CreateAsync(input);

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

            var result = await _identityUserAppService.UpdateAsync(johnNash.Id, input);

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

        private async Task<IdentityUser> GetUserAsync(string userName)
        {
            return (await _userRepository.GetListAsync()).First(u => u.UserName == userName);
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
