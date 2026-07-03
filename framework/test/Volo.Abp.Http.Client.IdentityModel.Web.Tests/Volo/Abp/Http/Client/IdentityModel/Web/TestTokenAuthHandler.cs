using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

public class TestTokenAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "TestToken";
    public const string TestAccessToken = "test-access-token";

    public TestTokenAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // The current HttpContext.User already carries the principal set up by the test.
        // This handler only makes an "access_token" retrievable via HttpContext.GetTokenAsync.
        var properties = new AuthenticationProperties();
        properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = TestAccessToken } });

        var ticket = new AuthenticationTicket(Context.User, properties, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
