using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Authorization;

public class AuthTestController_Tests : AspNetCoreMvcTestBase
{
    private readonly FakeUserClaims _fakeRequiredService;

    public AuthTestController_Tests()
    {
        _fakeRequiredService = GetRequiredService<FakeUserClaims>();
    }

    [Fact]
    public async Task Should_Call_Anonymous_Method_Without_Authentication()
    {
        var result = await GetResponseAsStringAsync("/AuthTest/AnonymousTest");
        result.ShouldBe("OK");
    }

    [Fact]
    public async Task Should_Call_Simple_Authorized_Method_With_Authenticated_User()
    {
        _fakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

        var result = await GetResponseAsStringAsync("/AuthTest/SimpleAuthorizationTest");
        result.ShouldBe("OK");
    }

    [Fact]
    public async Task Custom_Claim_Policy_Should_Work_With_Right_Claim_Provided()
    {
        _fakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString()),
                new Claim("MyCustomClaimType", "42")
            });

        var result = await GetResponseAsStringAsync("/AuthTest/CustomPolicyTest");
        result.ShouldBe("OK");
    }

    [Fact]
    public async Task Custom_Claim_Policy_Should_Not_Work_With_Wrong_Claim_Value()
    {
        _fakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString()),
                new Claim("MyCustomClaimType", "43")
            });

        await GetResponseAsStringAsync("/AuthTest/CustomPolicyTest", HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Should_Authorize_For_Defined_And_Allowed_Permission()
    {
        _fakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

        var result = await GetResponseAsStringAsync("/AuthTest/PermissionTest");
        result.ShouldBe("OK");
    }

    [Fact]
    public async Task Custom_And_Policy_Should_Not_Work_When_Permissions_Not_Granted()
    {
        _fakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

        var response = await GetResponseAsync("/AuthTest/Custom_And_PolicyTest", HttpStatusCode.Forbidden, xmlHttpRequest: true);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Custom_Or_Policy_Should_Work_When_Permissions_Are_Granted()
    {
        _fakeRequiredService.Claims.AddRange(new[]
        {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

        var result = await GetResponseAsStringAsync("/AuthTest/Custom_Or_PolicyTest");
        result.ShouldBe("OK");
    }
}
