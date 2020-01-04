﻿using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityUserAppService_Tests : AbpIdentityApplicationTestBase
    {
        private readonly IIdentityUserAppService _userAppService;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityTestData _testData;

        public IdentityUserAppService_Tests()
        {
            _userAppService = GetRequiredService<IIdentityUserAppService>();
            _userRepository = GetRequiredService<IIdentityUserRepository>();
            _testData = GetRequiredService<IdentityTestData>();
        }

        [Fact]
        public async Task GetAsync()
        {
            //Arrange

            var johnNash = GetUser("john.nash");

            //Act

            var result = await _userAppService.GetAsync(johnNash.Id).ConfigureAwait(false);

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

            var result = await _userAppService.GetListAsync(new GetIdentityUsersInput()).ConfigureAwait(false);

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
                Password = "123qwE4r*",
                RoleNames = new[] { "moderator" }
            };

            //Act

            var result = await _userAppService.CreateAsync(input).ConfigureAwait(false);

            //Assert

            result.Id.ShouldNotBe(Guid.Empty);
            result.UserName.ShouldBe(input.UserName);
            result.Email.ShouldBe(input.Email);
            result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            result.PhoneNumber.ShouldBe(input.PhoneNumber);

            var user = await _userRepository.GetAsync(result.Id).ConfigureAwait(false);
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

            var johnNash = GetUser("john.nash");

            var input = new IdentityUserUpdateDto
            {
                UserName = johnNash.UserName,
                LockoutEnabled = true,
                TwoFactorEnabled = true,
                PhoneNumber = CreateRandomPhoneNumber(),
                Password = "123qwe4R*",
                Email = CreateRandomEmail(),
                RoleNames = new[] { "admin", "moderator" },
                ConcurrencyStamp = johnNash.ConcurrencyStamp,
                Surname = johnNash.Surname,
                Name = johnNash.Name
            };

            //Act

            var result = await _userAppService.UpdateAsync(johnNash.Id, input).ConfigureAwait(false);

            //Assert

            result.Id.ShouldBe(johnNash.Id);
            result.UserName.ShouldBe(input.UserName);
            result.Email.ShouldBe(input.Email);
            result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            result.PhoneNumber.ShouldBe(input.PhoneNumber);

            var user = await _userRepository.GetAsync(result.Id).ConfigureAwait(false);
            user.Id.ShouldBe(result.Id);
            user.UserName.ShouldBe(input.UserName);
            user.Email.ShouldBe(input.Email);
            user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            user.PhoneNumber.ShouldBe(input.PhoneNumber);
            user.Roles.Count.ShouldBe(2);
        }


        [Fact]
        public async Task UpdateAsync_Concurrency_Exception()
        {
            //Get user
            var johnNash = await _userAppService.GetAsync(_testData.UserJohnId).ConfigureAwait(false);
            
            //Act

            var input = new IdentityUserUpdateDto
            {
                Name = "John-updated",
                Surname = "Nash-updated",
                UserName = johnNash.UserName,
                LockoutEnabled = true,
                TwoFactorEnabled = true,
                PhoneNumber = CreateRandomPhoneNumber(),
                Email = CreateRandomEmail(),
                RoleNames = new[] { "admin", "moderator" },
                ConcurrencyStamp = johnNash.ConcurrencyStamp
            };

            await _userAppService.UpdateAsync(johnNash.Id, input).ConfigureAwait(false);

            //Second update with same input will throw exception because the entity has been modified
            (await Assert.ThrowsAsync<AbpIdentityResultException>(async () =>
            {
                await _userAppService.UpdateAsync(johnNash.Id, input).ConfigureAwait(false);
            }).ConfigureAwait(false)).Message.ShouldContain("Optimistic concurrency failure");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            //Arrange

            var johnNash = GetUser("john.nash");

            //Act

            await _userAppService.DeleteAsync(johnNash.Id).ConfigureAwait(false);

            //Assert

            FindUser("john.nash").ShouldBeNull();
        }

        [Fact]
        public async Task GetRolesAsync()
        {
            //Arrange

            var johnNash = GetUser("john.nash");

            //Act

            var result = await _userAppService.GetRolesAsync(johnNash.Id).ConfigureAwait(false);

            //Assert

            result.Items.Count.ShouldBe(2);
            result.Items.ShouldContain(r => r.Name == "moderator");
            result.Items.ShouldContain(r => r.Name == "supporter");
        }

        [Fact]
        public async Task UpdateRolesAsync()
        {
            //Arrange

            var johnNash = GetUser("john.nash");

            //Act

            await _userAppService.UpdateRolesAsync(
                johnNash.Id,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = new[] { "admin", "moderator" }
                }
            ).ConfigureAwait(false);

            //Assert

            var roleNames = await _userRepository.GetRoleNamesAsync(johnNash.Id).ConfigureAwait(false);
            roleNames.Count.ShouldBe(2);
            roleNames.ShouldContain("admin");
            roleNames.ShouldContain("moderator");
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
