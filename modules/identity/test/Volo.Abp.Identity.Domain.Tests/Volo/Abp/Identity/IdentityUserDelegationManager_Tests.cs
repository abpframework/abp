using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserDelegationManager_Tests : AbpIdentityDomainTestBase
{
    protected IdentityUserDelegationManager IdentityUserDelegationManager { get; }
    protected IdentityTestData TestData { get; }
    
    public IdentityUserDelegationManager_Tests()
    {
        IdentityUserDelegationManager = GetRequiredService<IdentityUserDelegationManager>();
        TestData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        (await IdentityUserDelegationManager.GetListAsync(Guid.NewGuid(), Guid.NewGuid())).Count.ShouldBe(0);
        
        (await IdentityUserDelegationManager.GetListAsync(TestData.UserJohnId, null)).Count.ShouldBe(2);
        
        (await IdentityUserDelegationManager.GetListAsync(null, TestData.UserDavidId)).Count.ShouldBe(3);
        
        (await IdentityUserDelegationManager.GetListAsync(TestData.UserNeoId, TestData.UserDavidId)).Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetActiveDelegationsAsync()
    {
        var activeDelegations = await IdentityUserDelegationManager.GetActiveDelegationsAsync(TestData.UserDavidId);
        activeDelegations.Count.ShouldBe(2);
        activeDelegations[0].SourceUserId.ShouldBe(TestData.UserJohnId);
        activeDelegations[0].TargetUserId.ShouldBe(TestData.UserDavidId);
        activeDelegations[1].SourceUserId.ShouldBe(TestData.UserNeoId);
        activeDelegations[1].TargetUserId.ShouldBe(TestData.UserDavidId);
    }

    [Fact]
    public async Task FindActiveDelegationByIdAsync()
    {
        var activeDelegations = await IdentityUserDelegationManager.GetActiveDelegationsAsync(TestData.UserDavidId);
        var activeDelegation = await IdentityUserDelegationManager.FindActiveDelegationByIdAsync(activeDelegations[0].Id);
        activeDelegation.ShouldNotBeNull();
        activeDelegation.SourceUserId.ShouldBe(TestData.UserJohnId);
        activeDelegation.TargetUserId.ShouldBe(TestData.UserDavidId);
    }

    [Fact]
    public async Task DelegateNewUserAsync()
    {
        await Should.ThrowAsync<BusinessException>(IdentityUserDelegationManager.DelegateNewUserAsync(
            TestData.UserJohnId,
            TestData.UserJohnId,
            DateTime.Now.AddDays(-1),
            DateTime.Now));

        await IdentityUserDelegationManager.DelegateNewUserAsync(
            TestData.UserJohnId,
            TestData.UserBobId,
            DateTime.Now.AddDays(-1),
            DateTime.Now.AddDays(1));
        
        (await IdentityUserDelegationManager.GetActiveDelegationsAsync(TestData.UserBobId)).Count.ShouldBe(1);
    }
}