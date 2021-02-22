using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
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
            return Json(new {Now=DateTime.Now });
        }
    }
}
