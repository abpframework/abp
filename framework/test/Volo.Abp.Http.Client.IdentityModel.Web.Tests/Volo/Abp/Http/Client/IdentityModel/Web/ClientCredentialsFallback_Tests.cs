using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;
using Xunit;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

public class ClientCredentialsFallback_Tests
{
    [Theory]
    [InlineData(false, null)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    [InlineData(true, null)]
    [InlineData(true, true)]
    [InlineData(true, false)]
    public async Task Http_Request_Without_User_Token_Must_Not_Use_Service_Credentials(bool authenticated, bool? useCurrentToken)
    {
        var authentication = Substitute.For<IIdentityModelAuthenticationService>();
        var tokenProvider = Substitute.For<IAbpAccessTokenProvider>();
        tokenProvider.GetTokenAsync().Returns(Task.FromResult<string?>(null));
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(authenticated ? "cookie" : null))
            }
        };
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.test/users");
        var configuration = new RemoteServiceConfiguration("https://api.test")
            .SetUseCurrentAccessToken(useCurrentToken);

        await new HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(authentication, tokenProvider, accessor)
            .Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, request, configuration, "Users"));

        await authentication.DidNotReceiveWithAnyArgs().TryAuthenticateAsync(default!, default);
        request.Headers.Authorization.ShouldBeNull();
        client.DefaultRequestHeaders.Authorization.ShouldBeNull();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Constructor_Without_HttpContextAccessor_Uses_Current_HttpContext(bool httpContext)
    {
        var authentication = Substitute.For<IIdentityModelAuthenticationService>();
        var tokenProvider = Substitute.For<IAbpAccessTokenProvider>();
        tokenProvider.GetTokenAsync().Returns(Task.FromResult<string?>(null));
        var accessor = new HttpContextAccessor { HttpContext = httpContext ? new DefaultHttpContext() : null };
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.test/users");
        var configuration = new RemoteServiceConfiguration("https://api.test");

        await new HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(authentication, tokenProvider)
            .Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, request, configuration, "Users"));

        await authentication.Received(httpContext ? 0 : 1).TryAuthenticateAsync(client, Arg.Any<string>());
        accessor.HttpContext = null;
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task Background_Or_Explicitly_Trusted_Integration_Can_Use_Service_Credentials(bool httpContext, bool useCurrentToken)
    {
        var authentication = Substitute.For<IIdentityModelAuthenticationService>();
        var tokenProvider = Substitute.For<IAbpAccessTokenProvider>();
        tokenProvider.GetTokenAsync().Returns(Task.FromResult<string?>(null));
        var accessor = new HttpContextAccessor { HttpContext = httpContext ? new DefaultHttpContext() : null };
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.test/users");
        var configuration = new RemoteServiceConfiguration("https://api.test")
            .SetUseCurrentAccessToken(useCurrentToken)
            .SetIdentityClient("InternalIntegration")
            .SetAllowClientCredentialsInHttpContext(httpContext);

        await new HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(authentication, tokenProvider, accessor)
            .Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, request, configuration, "Users"));

        await authentication.Received(1).TryAuthenticateAsync(client, "InternalIntegration");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Current_Token_Takes_Precedence_Over_Service_Credentials(bool allowClientCredentials)
    {
        var authentication = Substitute.For<IIdentityModelAuthenticationService>();
        var tokenProvider = Substitute.For<IAbpAccessTokenProvider>();
        tokenProvider.GetTokenAsync().Returns("user-token");
        var accessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.test/users");
        var configuration = new RemoteServiceConfiguration("https://api.test")
            .SetAllowClientCredentialsInHttpContext(allowClientCredentials);

        await new HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(authentication, tokenProvider, accessor)
            .Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, request, configuration, "Users"));

        request.Headers.Authorization.ShouldBe(new AuthenticationHeaderValue("Bearer", "user-token"));
        await authentication.DidNotReceiveWithAnyArgs().TryAuthenticateAsync(default!, default);
    }
}
