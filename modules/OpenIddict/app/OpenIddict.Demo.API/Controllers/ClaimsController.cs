using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenIddict.Demo.API.Controllers;

[ApiController]
[Authorize]
[Route("api/claims")]
public class ClaimsController : Controller
{
    public JsonResult Get()
    {
        return Json(User.Claims.Select(x => new {Type = x.Type, Value = x.Value}));
    }
}