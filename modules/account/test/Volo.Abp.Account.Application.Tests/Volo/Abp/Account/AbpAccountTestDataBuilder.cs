using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account;

public class AbpAccountTestDataBuilder : ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IIdentityUserRepository _userRepository;
    private readonly AccountTestData _testData;

    public AbpAccountTestDataBuilder(
        AccountTestData testData,
        IGuidGenerator guidGenerator,
        IIdentityUserRepository userRepository)
    {
        _testData = testData;
        _guidGenerator = guidGenerator;
        _userRepository = userRepository;
    }

    public async Task Build()
    {
        await AddUsers();
    }

    private async Task AddUsers()
    {
        var john = new IdentityUser(_testData.UserJohnId, "john.nash", "john.nash@abp.io");
        john.AddLogin(new UserLoginInfo("github", "john", "John Nash"));
        john.AddLogin(new UserLoginInfo("twitter", "johnx", "John Nash"));
        john.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
        john.SetToken("test-provider", "test-name", "test-value");
        await _userRepository.InsertAsync(john);
    }
}
