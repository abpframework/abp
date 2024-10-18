using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc;

public static class FakeAuthenticationSchemeDefaults
{
    public static string Scheme => "FakeAuthenticationScheme";
}

public static class FakeAuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddFakeAuthentication(this AuthenticationBuilder builder)
    {
        return builder.AddScheme<FakeAuthenticationOptions, FakeAuthenticationHandler>(FakeAuthenticationSchemeDefaults.Scheme, _ => { });
    }
}

public class FakeAuthenticationOptions : AuthenticationSchemeOptions
{

}

public class FakeAuthenticationHandler : AuthenticationHandler<FakeAuthenticationOptions>
{
    private readonly FakeUserClaims _fakeUserClaims;

    [Obsolete]
    public FakeAuthenticationHandler(
        IOptionsMonitor<FakeAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        FakeUserClaims fakeUserClaims) : base(options, logger, encoder, clock)
    {
        _fakeUserClaims = fakeUserClaims;
    }

    public FakeAuthenticationHandler(
        IOptionsMonitor<FakeAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        FakeUserClaims fakeUserClaims)
        : base(options, logger, encoder)
    {
        _fakeUserClaims = fakeUserClaims;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_fakeUserClaims.Claims.Any())
        {
            return Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(new ClaimsIdentity(_fakeUserClaims.Claims,
                        FakeAuthenticationSchemeDefaults.Scheme)),
                    FakeAuthenticationSchemeDefaults.Scheme)));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
