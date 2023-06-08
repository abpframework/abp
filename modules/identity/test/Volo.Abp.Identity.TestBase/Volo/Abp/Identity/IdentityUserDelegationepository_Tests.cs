using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class IdentityUserDelegationepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityUserDelegationRepository IdentityUserDelegationRepository { get; }
    protected IdentityTestData TestData { get; }
    
    public IdentityUserDelegationepository_Tests()
    {
        UserRepository = GetRequiredService<IIdentityUserRepository>();
        IdentityUserDelegationRepository = GetRequiredService<IIdentityUserDelegationRepository>();
        TestData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        (await IdentityUserDelegationRepository.GetListAsync(Guid.NewGuid(), Guid.NewGuid())).Count.ShouldBe(0);
        
        (await IdentityUserDelegationRepository.GetListAsync(TestData.UserJohnId, null)).Count.ShouldBe(2);
        
        (await IdentityUserDelegationRepository.GetListAsync(null, TestData.UserDavidId)).Count.ShouldBe(3);
        
        (await IdentityUserDelegationRepository.GetListAsync(TestData.UserNeoId, TestData.UserDavidId)).Count.ShouldBe(1);
    }
    
    [Fact]
    public async Task GetActiveDelegationsAsync()
    {
        var activeDelegations = await IdentityUserDelegationRepository.GetActiveDelegationsAsync(TestData.UserDavidId);
        activeDelegations.Count.ShouldBe(2);
        activeDelegations[0].SourceUserId.ShouldBe(TestData.UserJohnId);
        activeDelegations[0].TargetUserId.ShouldBe(TestData.UserDavidId);
        activeDelegations[1].SourceUserId.ShouldBe(TestData.UserNeoId);
        activeDelegations[1].TargetUserId.ShouldBe(TestData.UserDavidId);
    }
    
    [Fact]
    public async Task GetActiveDelegationOrNullAsync()
    {
        var activeDelegations = await IdentityUserDelegationRepository.GetActiveDelegationsAsync(TestData.UserDavidId);
        var activeDelegation = await IdentityUserDelegationRepository.FindActiveDelegationByIdAsync(activeDelegations[0].Id);
        activeDelegation.ShouldNotBeNull();
        activeDelegation.SourceUserId.ShouldBe(TestData.UserJohnId);
        activeDelegation.TargetUserId.ShouldBe(TestData.UserDavidId);
    }
}