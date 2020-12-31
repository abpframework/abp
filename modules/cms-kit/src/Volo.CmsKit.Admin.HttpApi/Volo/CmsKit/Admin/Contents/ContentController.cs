using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Admin.Contents;

namespace Volo.CmsKit.Admin.HttpApi.Volo.CmsKit.Admin.Contents
{
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

        public Task<ContentDto> CreateAsync(ContentCreateDto input)
        {
            return ContentAdminAppService.CreateAsync(input);
        }

        public Task DeleteAsync(Guid id)
        {
            return ContentAdminAppService.DeleteAsync(id);
        }

        public Task<ContentDto> GetAsync(Guid id)
        {
            return ContentAdminAppService.GetAsync(id);
        }

        public Task<PagedResultDto<ContentDto>> GetListAsync(ContentGetListInput input)
        {
            return ContentAdminAppService.GetListAsync(input);
        }

        public Task<ContentDto> UpdateAsync(Guid id, ContentUpdateDto input)
        {
            return ContentAdminAppService.UpdateAsync(id, input);
        }
    }
}
