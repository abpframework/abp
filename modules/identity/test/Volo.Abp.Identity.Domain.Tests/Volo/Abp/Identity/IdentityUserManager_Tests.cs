using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
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
                    _lookupNormalizer.NormalizeName("david"));
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
    }
}
