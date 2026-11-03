using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class IdentitySessionCookieValidation_Tests : AbpIdentityAspNetCoreTestBase
{
    [Fact]
    public async Task Should_Authenticate_The_Cookie_When_The_Session_Is_Valid()
    {
        var cookie = await LoginAsync();

        using (var response = await GetCurrentUserAsync(cookie))
        {
            (await response.Content.ReadAsStringAsync()).ShouldStartWith("admin|");
        }
    }

    [Fact]
    public async Task Should_Reject_The_Cookie_Without_Writing_It_When_The_Session_Is_Revoked()
    {
        var cookie = await LoginAsync();
        var sessionId = await GetSessionIdAsync(cookie);

        GetRequiredService<FakeIdentitySessionChecker>().RevokedSessionIds.Add(sessionId);

        using (var response = await GetCurrentUserAsync(cookie))
        {
            (await response.Content.ReadAsStringAsync()).ShouldBe("anonymous");
            response.Headers.Contains("Set-Cookie").ShouldBeFalse();
        }
    }

    [Fact]
    public async Task Account_Switch_Cookie_Should_Survive_An_In_Flight_Request_Carrying_The_Old_Revoked_Cookie()
    {
        var oldCookie = await LoginAsync();
        var oldSessionId = await GetSessionIdAsync(oldCookie);

        var newCookie = await SwitchAccountAsync(oldCookie, "john.nash");
        newCookie.ShouldNotBeNull();
        newCookie.ShouldNotBe(oldCookie);

        // The switch revoked the old session.
        GetRequiredService<FakeIdentitySessionChecker>().RevokedSessionIds.Add(oldSessionId);

        // An in-flight request still carrying the old cookie must be rejected without emitting a
        // delete-cookie that would wipe the freshly issued account-switch cookie.
        using (var response = await GetCurrentUserAsync(oldCookie))
        {
            (await response.Content.ReadAsStringAsync()).ShouldBe("anonymous");
            response.Headers.Contains("Set-Cookie").ShouldBeFalse();
        }

        using (var response = await GetCurrentUserAsync(newCookie))
        {
            (await response.Content.ReadAsStringAsync()).ShouldStartWith("john.nash|");
        }
    }

    private async Task<string> SwitchAccountAsync(string cookie, string userName)
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/signin-test/switch-account?userName=" + userName))
        {
            requestMessage.Headers.Add("Cookie", cookie);
            using (var response = await Client.SendAsync(requestMessage))
            {
                (await response.Content.ReadAsStringAsync()).ShouldBe("Succeeded");
                var cookieName = CookieAuthenticationDefaults.CookiePrefix + IdentityConstants.ApplicationScheme;
                return response.Headers.GetValues("Set-Cookie")
                    .Select(x => x.Split(';')[0])
                    .Last(x => x.StartsWith(cookieName) && x.Length > cookieName.Length + 1);
            }
        }
    }

    private async Task<string> LoginAsync()
    {
        using (var response = await Client.GetAsync("api/signin-test/password?userName=admin&password=1q2w3E*"))
        {
            (await response.Content.ReadAsStringAsync()).ShouldBe("Succeeded");
            var cookie = response.Headers.GetValues("Set-Cookie").First(x => x.StartsWith(CookieAuthenticationDefaults.CookiePrefix + IdentityConstants.ApplicationScheme));
            return cookie.Split(';')[0];
        }
    }

    private async Task<string> GetSessionIdAsync(string cookie)
    {
        using (var response = await GetCurrentUserAsync(cookie))
        {
            return (await response.Content.ReadAsStringAsync()).Split('|')[1];
        }
    }

    private async Task<HttpResponseMessage> GetCurrentUserAsync(string cookie)
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/signin-test/current-user"))
        {
            requestMessage.Headers.Add("Cookie", cookie);
            return await Client.SendAsync(requestMessage);
        }
    }
}
