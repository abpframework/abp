using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging;

namespace Volo.BloggingTestApp.Controllers
{
    public class HomeController : AbpController
    {
        private readonly BloggingUrlOptions _blogOptions;

        public HomeController(IOptions<BloggingUrlOptions> blogOptions)
        {
            _blogOptions = blogOptions.Value;
        }
        public ActionResult Index()
        {
            var urlPrefix = _blogOptions.RoutePrefix;
            return Redirect(urlPrefix);
        }
    }
}
