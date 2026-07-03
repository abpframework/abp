using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Client.IdentityModel.Web.Tests;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

public class HttpContextAbpAccessTokenProvider_Tests : AbpIntegratedTest<AbpHttpClientIdentityModelWebTestModule>
{
    private readonly IAbpAccessTokenProvider _accessTokenProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAbpAccessTokenProvider_Tests()
    {
        _accessTokenProvider = GetRequiredService<IAbpAccessTokenProvider>();
        _httpContextAccessor = GetRequiredService<IHttpContextAccessor>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.Services.AddHttpContextAccessor();
    }

    [Fact]
    public async Task Should_Forward_Token_For_Authenticated_User()
    {
        var identity = new ClaimsIdentity(TestTokenAuthHandler.SchemeName);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, "d3c1a6b0-1234-4a6f-9e3b-8c2f1d4a5b6e"));
        _httpContextAccessor.HttpContext = CreateHttpContext(new ClaimsPrincipal(identity));

        (await _accessTokenProvider.GetTokenAsync()).ShouldBe(TestTokenAuthHandler.TestAccessToken);
    }

    [Fact]
    public async Task Should_Forward_Token_For_Authenticated_Client_Without_User()
    {
        // client_credentials token: authenticated identity but no user id claim.
        var identity = new ClaimsIdentity(TestTokenAuthHandler.SchemeName);
        identity.AddClaim(new Claim(AbpClaimTypes.ClientId, "test-client"));
        _httpContextAccessor.HttpContext = CreateHttpContext(new ClaimsPrincipal(identity));

        (await _accessTokenProvider.GetTokenAsync()).ShouldBe(TestTokenAuthHandler.TestAccessToken);
    }

    [Fact]
    public async Task Should_Not_Forward_Token_For_Anonymous_Request()
    {
        // No authentication type => Identity.IsAuthenticated is false (e.g. before authentication middleware).
        _httpContextAccessor.HttpContext = CreateHttpContext(new ClaimsPrincipal(new ClaimsIdentity()));

        (await _accessTokenProvider.GetTokenAsync()).ShouldBeNull();
    }

    private static DefaultHttpContext CreateHttpContext(ClaimsPrincipal user)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication(TestTokenAuthHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, TestTokenAuthHandler>(TestTokenAuthHandler.SchemeName, _ => { });

        return new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider(),
            User = user
        };
    }
}
