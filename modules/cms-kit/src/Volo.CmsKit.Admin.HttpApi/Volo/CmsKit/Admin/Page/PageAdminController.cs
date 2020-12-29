using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.CmsKit.Admin.Pages;

namespace Volo.CmsKit.Admin.Page
{
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-admin/pages")]
    public class PageAdminController : CmsKitAdminController, IPageAdminAppService
    {
        protected readonly IPageAdminAppService PageAdminAppService;

        public PageAdminController(IPageAdminAppService pageAdminAppService)
        {
            PageAdminAppService = pageAdminAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PageDto> GetAsync(Guid id)
        {
            return PageAdminAppService.GetAsync(id);
        }
        
        [HttpPost]
        [Route("create")]
        public virtual Task<PageDto> CreatePageAsync(CreatePageInputDto input)
        {
            return PageAdminAppService.CreatePageAsync(input);
        }

        [HttpPost]
        [Route("create-with-content")]
        public virtual Task<PageDto> CreatePageWithContentAsync(CreatePageWithContentInputDto input)
        {
            return PageAdminAppService.CreatePageWithContentAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PageDto> UpdatePageAsync(Guid id, UpdatePageInputDto input)
        {
            return PageAdminAppService.UpdatePageAsync(id, input);
        }

        [HttpPost]
        [Route("does-url-exist")]
        public virtual Task<bool> DoesUrlExistAsync(CheckUrlInputDto input)
        {
            return PageAdminAppService.DoesUrlExistAsync(input);
        }

        [HttpPut]
        [Route("update-content/{id}")]
        public virtual Task UpdatePageContentAsync(Guid id, UpdatePageContentInputDto input)
        {
            return PageAdminAppService.UpdatePageContentAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return PageAdminAppService.DeleteAsync(id);
        }
    }
}