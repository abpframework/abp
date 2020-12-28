using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Controllers.Pages
{
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit/pages")]
    public class PageController : CmsKitControllerBase, IPageAppService
    {
        protected readonly IPageAppService PageAppService;

        public PageController(IPageAppService pageAppService)
        {
            PageAppService = pageAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PageDto> GetAsync(Guid id)
        {
            return PageAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("url/{url}")]
        public Task<PageDto> GetByUrlAsync(string url)
        {
            return PageAppService.GetByUrlAsync(url);
        }

        [HttpPost]
        [Route("create")]
        public virtual Task<PageDto> CreatePageAsync(CreatePageInputDto input)
        {
            return PageAppService.CreatePageAsync(input);
        }

        [HttpPost]
        [Route("create-with-content")]
        public virtual Task<PageDto> CreatePageWithContentAsync(CreatePageWithContentInputDto input)
        {
            return PageAppService.CreatePageWithContentAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PageDto> UpdatePageAsync(Guid id, UpdatePageInputDto input)
        {
            return PageAppService.UpdatePageAsync(id, input);
        }

        [HttpPost]
        [Route("does-url-exist")]
        public virtual Task<bool> DoesUrlExistAsync(CheckUrlInputDto input)
        {
            return PageAppService.DoesUrlExistAsync(input);
        }

        [HttpPut]
        [Route("update-content/{id}")]
        public virtual Task UpdatePageContentAsync(Guid id, UpdatePageContentInputDto input)
        {
            return PageAppService.UpdatePageContentAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return PageAppService.DeleteAsync(id);
        }
    }
}