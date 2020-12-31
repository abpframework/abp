using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Admin.Contents;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Contents
{
    [Authorize(CmsKitAdminPermissions.Contents.Default)]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-admin/contents")]
    public class ContentController : CmsKitAdminController, IContentAdminAppService
    {
        public ContentController(IContentAdminAppService contentAdminAppService)
        {
            ContentAdminAppService = contentAdminAppService;
        }

        protected IContentAdminAppService ContentAdminAppService { get; }

        [Authorize(CmsKitAdminPermissions.Contents.Create)]
        public Task<ContentDto> CreateAsync(ContentCreateDto input)
        {
            return ContentAdminAppService.CreateAsync(input);
        }

        [Authorize(CmsKitAdminPermissions.Contents.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return ContentAdminAppService.DeleteAsync(id);
        }

        [Authorize(CmsKitAdminPermissions.Contents.Default)]
        public Task<ContentDto> GetAsync(Guid id)
        {
            return ContentAdminAppService.GetAsync(id);
        }

        [Authorize(CmsKitAdminPermissions.Contents.Default)]
        public Task<PagedResultDto<ContentDto>> GetListAsync(ContentGetListInput input)
        {
            return ContentAdminAppService.GetListAsync(input);
        }

        [Authorize(CmsKitAdminPermissions.Contents.Update)]
        public Task<ContentDto> UpdateAsync(Guid id, ContentUpdateDto input)
        {
            return ContentAdminAppService.UpdateAsync(id, input);
        }
    }
}
