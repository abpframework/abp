using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Public.Contents;
using Volo.CmsKit.Web.Contents;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Contents
{
    [ViewComponent(Name = "CmsContent")]
    public class ContentViewComponent : AbpViewComponent
    {
        private readonly IContentRenderer contentRenderer;
        private readonly IContentAppService contentAppService;

        public ContentViewComponent(
            IContentRenderer contentRenderer,
            IContentAppService contentAppService)
        {
            this.contentRenderer = contentRenderer;
            this.contentAppService = contentAppService;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(
            string entityType,
            string entityId)
        {
            var content = await contentAppService.GetAsync(new GetContentInput
            {
                EntityId = entityId,
                EntityType = entityType
            });

            var viewModel = new ContentViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                ContentId = content.Id,
                Rendered = await contentRenderer.RenderAsync(content.Value)
            };

            return View("~/Pages/CmsKit/Shared/Components/Contents/Default.cshtml", viewModel);
        }

        public class ContentViewModel
        {
            public Guid ContentId { get; set; }
            public string EntityType { get; set; }

            public string EntityId { get; set; }

            public string Rendered { get; set; }
        }
    }
}
