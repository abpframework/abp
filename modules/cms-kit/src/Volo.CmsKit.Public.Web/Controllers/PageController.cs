using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web.Controllers
{
    [RequiresGlobalFeature(typeof(PagesFeature))]
    public class PageController : CmsKitPublicControllerBase
    {
        protected IPageAppService PageAppService { get; }

        public PageController(IPageAppService pageAppService)
        {
            PageAppService = pageAppService;
        }

        //[HttpGet("/{*url}", Order = int.MaxValue)]
        public async Task<IActionResult> IndexAsync(string url)
        {
            var page = await PageAppService.FindByUrlAsync(url);

            if (page == null)
            {
                return NotFound();
            }

            return View("~/Views/Page/Index.cshtml", page);
        }
    }
}
