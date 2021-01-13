using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages
{
    [ViewComponent(Name = "CmsDefaultPage")]
    public class DefaultPageViewComponent : AbpViewComponent
    {
        protected readonly IPageAppService PageAppService;

        public DefaultPageViewComponent(IPageAppService pageAppService)
        {
            PageAppService = pageAppService;
        }
        
        public virtual async Task<IViewComponentResult> InvokeAsync(Guid pageId, string title, string description)
        {
            var model = new PageViewModel
            {
                Id = pageId,
                Title = title
            };
            
            return View("~/Pages/CmsKit/Shared/Components/Pages/Default.cshtml", model);
        }
    }

    public class PageViewModel
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
    }
}