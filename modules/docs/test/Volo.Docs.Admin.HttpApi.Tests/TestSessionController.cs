using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Security.Claims;

namespace Volo.Docs.Admin;

[Route("tests/session")]
public class TestSessionController : ControllerBase
{
    [HttpGet("sign-in")]
    public async Task SignInAsync()
    {
        var identity = new ClaimsIdentity(new[] { new Claim(AbpClaimTypes.UserId, "5a095280-a434-48cc-b5d2-612b52a6be71") }, "DocsTest");
        await HttpContext.SignInAsync("DocsTest", new ClaimsPrincipal(identity));
    }

    [HttpGet("token")]
    public string Token([FromServices] IAntiforgery antiforgery)
    {
        return antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
    }
}
