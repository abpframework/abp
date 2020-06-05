using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Admin.Blogs;

namespace Volo.Blogging.Admin
{
    [RemoteService(Name = BloggingAdminRemoteServiceConsts.RemoteServiceName)]
    [Area("bloggingAdmin")]
    [Route("api/blogging/blogs/admin")]
    public class BlogManagementController : AbpController, IBlogManagementAppService
    {
        private readonly IBlogManagementAppService _blogManagementAppService;

        public BlogManagementController(IBlogManagementAppService blogManagementAppService)
        {
            _blogManagementAppService = blogManagementAppService;
        }

        [HttpGet]
        public async Task<ListResultDto<BlogDto>> GetListAsync()
        {
            return await _blogManagementAppService.GetListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<BlogDto> GetAsync(Guid id)
        {
            return await _blogManagementAppService.GetAsync(id);
        }

        [HttpPost]
        public async Task<BlogDto> Create(CreateBlogDto input)
        {
            return await _blogManagementAppService.Create(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<BlogDto> Update(Guid id, UpdateBlogDto input)
        {
            return await _blogManagementAppService.Update(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(Guid id)
        {
            await _blogManagementAppService.Delete(id);
        }
    }
}
