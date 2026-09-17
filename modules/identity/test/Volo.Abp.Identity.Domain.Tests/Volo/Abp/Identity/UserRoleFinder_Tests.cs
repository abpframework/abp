using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity;

public class UserRoleFinder_Tests : AbpIdentityDomainTestBase
{
    private readonly IUserRoleFinder _userRoleFinder;
    private readonly IdentityTestData _testData;

    public UserRoleFinder_Tests()
    {
        _userRoleFinder = GetRequiredService<IUserRoleFinder>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task GetRoleNamesAsync()
    {
        var roleNames = await _userRoleFinder.GetRoleNamesAsync(_testData.UserJohnId);
        roleNames.ShouldNotBeEmpty();
        roleNames.ShouldContain(x => x == "moderator");
        roleNames.ShouldContain(x => x == "supporter");
    }

    [Fact]
    public async Task SearchUserAsync()
    {
        var userResults = await _userRoleFinder.SearchUserAsync("john");
        userResults.ShouldNotBeEmpty();
        userResults.ShouldContain(x => x.Id == _testData.UserJohnId);
    }

    [Fact]
    public async Task SearchRoleAsync()
    {
        var roleResults = await _userRoleFinder.SearchRoleAsync("moderator");
        roleResults.ShouldNotBeEmpty();
        roleResults.ShouldContain(x => x.RoleName == "moderator");
    }

    [Fact]
    public async Task SearchUserByIdsAsync()
    {
        var userResults = await _userRoleFinder.SearchUserByIdsAsync(new[] { _testData.UserJohnId, _testData.UserBobId });
        userResults.ShouldNotBeEmpty();
        userResults.Count.ShouldBe(2);
        userResults.ShouldContain(x => x.Id == _testData.UserJohnId && x.UserName == "john.nash");
        userResults.ShouldContain(x => x.Id == _testData.UserBobId && x.UserName == "bob");
    }

    [Fact]
    public async Task SearchRoleByNamesAsync()
    {
        var roleResults = await _userRoleFinder.SearchRoleByNamesAsync(new[] { "moderator", "manager" });
        roleResults.ShouldNotBeEmpty();
        roleResults.Count.ShouldBe(2);
        roleResults.ShouldContain(x => x.RoleName == "moderator");
        roleResults.ShouldContain(x => x.RoleName == "manager");
    }
}
