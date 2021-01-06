using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Contents;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Contents
{
    [Authorize(CmsKitAdminPermissions.Contents.Default)]
    [RequiresGlobalFeature(typeof(ContentsFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-admin/contents")]
    public class ContentAdminController : CmsKitAdminController, IContentAdminAppService
    {
        protected IContentAdminAppService ContentAdminAppService { get; }

        public ContentAdminController(IContentAdminAppService contentAdminAppService)
        {
            ContentAdminAppService = contentAdminAppService;
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Contents.Create)]
        public Task<ContentDto> CreateAsync(ContentCreateDto input)
        {
            return ContentAdminAppService.CreateAsync(input);
        }

        [HttpDelete("{id}")]
        [Authorize(CmsKitAdminPermissions.Contents.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return ContentAdminAppService.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        [Authorize(CmsKitAdminPermissions.Contents.Default)]
        public Task<ContentDto> GetAsync(Guid id)
        {
            return ContentAdminAppService.GetAsync(id);
        }

        [HttpGet]
        [Authorize(CmsKitAdminPermissions.Contents.Default)]
        public Task<PagedResultDto<ContentDto>> GetListAsync(ContentGetListInput input)
        {
            return ContentAdminAppService.GetListAsync(input);
        }

        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Contents.Update)]
        public Task<ContentDto> UpdateAsync(Guid id, ContentUpdateDto input)
        {
            return ContentAdminAppService.UpdateAsync(id, input);
        }
    }
}
