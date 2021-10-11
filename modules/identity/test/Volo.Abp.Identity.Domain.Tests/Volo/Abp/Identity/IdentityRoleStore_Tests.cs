using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityRoleStore_Tests : AbpIdentityDomainTestBase
    {
        private readonly IdentityRoleStore _identityRoleStore;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly IdentityTestData _testData;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public IdentityRoleStore_Tests()
        {
            _identityRoleStore = GetRequiredService<IdentityRoleStore>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _testData = GetRequiredService<IdentityTestData>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var roleId = Guid.NewGuid();
            var role = new IdentityRole(roleId, "teacher");
            (await _identityRoleStore.CreateAsync(role)).Succeeded.ShouldBeTrue();

            var teacher = await _identityRoleStore.FindByIdAsync(roleId.ToString());

            teacher.ShouldNotBeNull();
            teacher.Name.ShouldBe("teacher");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            role.IsDefault = true;
            await _identityRoleStore.UpdateAsync(role);

            role.IsDefault.ShouldBeTrue();
        }


        [Fact]
        public async Task DeleteAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            await _identityRoleStore.DeleteAsync(role);

            (await _identityRoleStore.FindByIdAsync(_testData.RoleModeratorId.ToString())).ShouldBeNull();
        }

        [Fact]
        public async Task GetRoleIdAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            (await _identityRoleStore.GetRoleIdAsync(role)).ShouldBe(_testData.RoleModeratorId.ToString());
        }

        [Fact]
        public async Task GetRoleNameAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            (await _identityRoleStore.GetRoleNameAsync(role)).ShouldBe(role.Name);
        }


        [Fact]
        public async Task SetRoleNameAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            await _identityRoleStore.SetRoleNameAsync(role, "teacher");

            role.Name.ShouldBe("teacher");
        }

        [Fact]
        public async Task FindByIdAsync()
        {
            var role = await _identityRoleStore.FindByIdAsync(_testData.RoleModeratorId.ToString());

            role.ShouldNotBeNull();
            role.Name.ShouldBe("moderator");
        }

        [Fact]
        public async Task FindByNameAsync()
        {
            var role = await _identityRoleStore.FindByNameAsync(_lookupNormalizer.NormalizeName("moderator"));

            role.ShouldNotBeNull();
            role.Name.ShouldBe("moderator");
        }

        [Fact]
        public async Task GetNormalizedRoleNameAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            (await _identityRoleStore.GetNormalizedRoleNameAsync(role)).ShouldBe(role.NormalizedName);
        }

        [Fact]
        public async Task SetNormalizedRoleNameAsync()
        {
            var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role.ShouldNotBeNull();

            await _identityRoleStore.SetNormalizedRoleNameAsync(role, _lookupNormalizer.NormalizeName("teacher"));

            role.NormalizedName.ShouldBe(_lookupNormalizer.NormalizeName("teacher"));
        }

        [Fact]
        public async Task GetClaimsAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
                role.ShouldNotBeNull();

                var claims = await _identityRoleStore.GetClaimsAsync(role);

                claims.ShouldNotBeEmpty();
                claims.ShouldContain(x => x.Type == "test-claim" && x.Value == "test-value");

                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task AddClaimAsync()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
                    role.ShouldNotBeNull();

                    role.Claims.Add(new IdentityRoleClaim(Guid.NewGuid(), role.Id, "my-claim", "my-value", role.TenantId));
                    //await _identityRoleStore.AddClaimAsync(role, new Claim("my-claim", "my-value"));

                    //role.Claims.ShouldContain(x => x.ClaimType == "my-claim" && x.ClaimValue == "my-value");

                    await uow.CompleteAsync();
                }

            }
            catch
            {
                throw;
            }
            
            var role2 = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
            role2.ShouldNotBeNull();
            role2.Claims.ShouldContain(x => x.ClaimType == "my-claim" && x.ClaimValue == "my-value");
        }

        [Fact]
        public async Task RemoveClaimAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
                role.ShouldNotBeNull();

                await _identityRoleStore.RemoveClaimAsync(role, new Claim("test-claim", "test-value"));

                role.Claims.ShouldNotContain(x => x.ClaimType == "test-claim" && x.ClaimValue == "test-value");

                await uow.CompleteAsync();
            }
        }
    }
}
