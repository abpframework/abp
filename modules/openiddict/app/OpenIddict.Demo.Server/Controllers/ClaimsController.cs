using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenIddict.Demo.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/claims")]
public class ClaimsController : Controller
{
    [HttpGet]
    public JsonResult Get()
    {
        return Json(User.Claims.Select(x => new {Type = x.Type, Value = x.Value}));
    }
}
