using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdenityClaimTypeManager_Tests : AbpIdentityDomainTestBase
    {
        private readonly IIdentityClaimTypeRepository _identityClaimTypeRepository;
        private readonly IdenityClaimTypeManager _claimTypeManager;
        private readonly IdentityTestData _testData;

        public IdenityClaimTypeManager_Tests()
        {
            _identityClaimTypeRepository = GetRequiredService<IIdentityClaimTypeRepository>();
            _claimTypeManager = GetRequiredService<IdenityClaimTypeManager>();
            _testData = GetRequiredService<IdentityTestData>();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var claimType = await _claimTypeManager.CreateAsync(new IdentityClaimType(Guid.NewGuid(), "Phone", false,
                false, null,
                null, null, IdentityClaimValueType.String)).ConfigureAwait(false);

            claimType.ShouldNotBeNull();
            claimType.Name.ShouldBe("Phone");
        }

        [Fact]
        public async Task Create_Name_Exist_Should_Exception()
        {
            await Assert.ThrowsAnyAsync<AbpException>(async () => await _claimTypeManager.CreateAsync(
                new IdentityClaimType(
                    Guid.NewGuid(), "Age")).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var ageClaim = await _identityClaimTypeRepository.FindAsync(_testData.AgeClaimId).ConfigureAwait(false);
            ageClaim.ShouldNotBeNull();
            ageClaim.Description = "this is age";

            var updatedAgeClaimType = await _claimTypeManager.UpdateAsync(ageClaim).ConfigureAwait(false);
            updatedAgeClaimType.ShouldNotBeNull();
            updatedAgeClaimType.Description.ShouldBe("this is age");
        }


        [Fact]
        public async Task Update_Name_Exist_Should_Exception()
        {
            await Assert.ThrowsAnyAsync<AbpException>(async () => await _claimTypeManager.UpdateAsync(
                new IdentityClaimType(
                    Guid.NewGuid(), "Age")).ConfigureAwait(false)).ConfigureAwait(false);
        }


        [Fact]
        public async Task Static_IdentityClaimType_Cant_Not_Update()
        {
            var phoneClaim = new IdentityClaimType(Guid.NewGuid(), "Phone", true, true);
            await _identityClaimTypeRepository.InsertAsync(phoneClaim).ConfigureAwait(false);

            await Assert.ThrowsAnyAsync<AbpException>(async () => await _claimTypeManager.UpdateAsync(phoneClaim).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
