using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityUserManager_Tests : AbpIdentityDomainTestBase
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IdentityTestData _testData;

        public IdentityUserManager_Tests()
        {
            _identityUserManager = GetRequiredService<IdentityUserManager>();
            _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _testData = GetRequiredService<IdentityTestData>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);

            user.ShouldNotBeNull();
            user.UserName.ShouldBe("john.nash");
        }

        [Fact]
        public async Task SetRolesAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                    _lookupNormalizer.NormalizeName("david")
                );

                user.ShouldNotBeNull();

                var identityResult = await _identityUserManager.SetRolesAsync(user, new List<string>()
                {
                    "moderator",
                });

                identityResult.Succeeded.ShouldBeTrue();
                user.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);

                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task SetRoles_Should_Remove_Other_Roles()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var roleSupporter =
                    await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("supporter"));
                roleSupporter.ShouldNotBeNull();

                var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                    _lookupNormalizer.NormalizeName("john.nash"));
                user.ShouldNotBeNull();

                var identityResult = await _identityUserManager.SetRolesAsync(user, new List<string>()
                {
                    "admin",
                });

                identityResult.Succeeded.ShouldBeTrue();
                user.Roles.ShouldNotContain(x => x.RoleId == _testData.RoleModeratorId);
                user.Roles.ShouldNotContain(x => x.RoleId == roleSupporter.Id);

                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task AddDefaultRolesAsync_In_Same_Uow()
        {
            await CreateRandomDefaultRoleAsync();

            using (var uow = _unitOfWorkManager.Begin())
            {
                var user = CreateRandomUser();

                (await _identityUserManager.CreateAsync(user)).CheckErrors();

                user.Roles.Count.ShouldBe(0);

                await _identityUserManager.AddDefaultRolesAsync(user);

                user.Roles.Count.ShouldBeGreaterThan(0);

                foreach (var roleId in user.Roles.Select(r => r.RoleId))
                {
                    var role = await _identityRoleRepository.GetAsync(roleId);
                    role.IsDefault.ShouldBe(true);
                }

                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task AddDefaultRolesAsync_In_Different_Uow()
        {
            await CreateRandomDefaultRoleAsync();

            Guid userId;

            using (var uow = _unitOfWorkManager.Begin())
            {
                var user = CreateRandomUser();
                userId = user.Id;

                (await _identityUserManager.CreateAsync(user)).CheckErrors();
                user.Roles.Count.ShouldBe(0);
                await uow.CompleteAsync();
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                var user = await _identityUserManager.GetByIdAsync(userId);

                await _identityUserManager.AddDefaultRolesAsync(user);
                user.Roles.Count.ShouldBeGreaterThan(0);

                foreach (var roleId in user.Roles.Select(r => r.RoleId))
                {
                    var role = await _identityRoleRepository.GetAsync(roleId);
                    role.IsDefault.ShouldBe(true);
                }

                await uow.CompleteAsync();
            }
        }

        private async Task CreateRandomDefaultRoleAsync()
        {
            await _identityRoleRepository.InsertAsync(
                new IdentityRole(
                    Guid.NewGuid(),
                    Guid.NewGuid().ToString()
                )
                {
                    IsDefault = true
                }
            );
        }

        private static IdentityUser CreateRandomUser()
        {
            return new IdentityUser(
                Guid.NewGuid(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString() + "@abp.io"
            );
        }
    }
}
