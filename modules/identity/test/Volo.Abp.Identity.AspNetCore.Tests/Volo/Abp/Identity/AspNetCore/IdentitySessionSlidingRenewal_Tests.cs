using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class IdentitySessionSlidingRenewal_Tests : AbpAspNetCoreIntegratedTestBase<ShortLivedCookieIdentityTestStartup>
{
    private static readonly string AuthenticationCookieName = CookieAuthenticationDefaults.CookiePrefix + IdentityConstants.ApplicationScheme;

    [Fact]
    public async Task Should_Renew_The_Cookie_When_The_Session_Is_Valid_Past_The_Half_Life()
    {
        var cookie = await LoginAsync();

        // Past the half-life of the 10s window, so a renewal is scheduled for a still-valid session.
        GetRequiredService<TestTimeProvider>().Advance(TimeSpan.FromSeconds(6));

        using (var response = await SendAsync("api/signin-test/current-user", cookie))
        {
            (await response.Content.ReadAsStringAsync()).ShouldStartWith("admin|");
            GetAuthCookieHeader(response).ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task Should_Not_Renew_The_Cookie_When_The_Session_Is_Already_Revoked_At_Auth_Time()
    {
        var cookie = await LoginAsync();
        var sessionId = await GetSessionIdAsync(cookie);

        GetRequiredService<FakeIdentitySessionChecker>().RevokedSessionIds.Add(sessionId);

        // Same instant the valid session would renew at, so the missing renewal is the fix, not expiry.
        GetRequiredService<TestTimeProvider>().Advance(TimeSpan.FromSeconds(6));

        using (var response = await SendAsync("api/signin-test/current-user", cookie))
        {
            (await response.Content.ReadAsStringAsync()).ShouldBe("anonymous");
            GetAuthCookieHeader(response).ShouldBeNull();
        }
    }

    private async Task<string> LoginAsync()
    {
        using (var response = await Client.GetAsync("api/signin-test/password?userName=admin&password=1q2w3E*"))
        {
            (await response.Content.ReadAsStringAsync()).ShouldBe("Succeeded");
            var cookie = response.Headers.GetValues("Set-Cookie").First(x => x.StartsWith(AuthenticationCookieName));
            return cookie.Split(';')[0];
        }
    }

    private async Task<string> GetSessionIdAsync(string cookie)
    {
        using (var response = await SendAsync("api/signin-test/current-user", cookie))
        {
            return (await response.Content.ReadAsStringAsync()).Split('|')[1];
        }
    }

    private async Task<HttpResponseMessage> SendAsync(string url, string cookie)
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
        {
            requestMessage.Headers.Add("Cookie", cookie);
            return await Client.SendAsync(requestMessage);
        }
    }

    private static string GetAuthCookieHeader(HttpResponseMessage response)
    {
        return response.Headers.TryGetValues("Set-Cookie", out var values)
            ? values.FirstOrDefault(x => x.StartsWith(AuthenticationCookieName))
            : null;
    }
}
