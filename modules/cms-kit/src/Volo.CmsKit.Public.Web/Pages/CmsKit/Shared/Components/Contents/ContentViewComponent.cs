using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
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
            var content = string.Empty;
            
            try
            {
                var contentDto = await contentAppService.GetAsync(new GetContentInput
                {
                    EntityId = entityId,
                    EntityType = entityType
                });

                content = contentDto.Value;
            }
            catch (EntityNotFoundException e)
            {
                // ContentDto can be null, we will render empty content.
            }

            var viewModel = new ContentViewModel
            {
                Value = await contentRenderer.RenderAsync(content)
            };

            return View("~/Pages/CmsKit/Shared/Components/Contents/Default.cshtml", viewModel);
        }

        public class ContentViewModel
        {
            public string Value { get; set; }
        }
    }
}
