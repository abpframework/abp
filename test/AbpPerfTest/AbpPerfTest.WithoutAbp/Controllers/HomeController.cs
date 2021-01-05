using Microsoft.AspNetCore.Mvc;

namespace AbpPerfTest.WithoutAbp.Controllers
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
