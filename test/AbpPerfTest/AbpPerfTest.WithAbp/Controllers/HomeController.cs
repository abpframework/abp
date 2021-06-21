using Microsoft.AspNetCore.Mvc;

namespace AbpPerfTest.WithAbp.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("/api/books/");
        }
    }
}
