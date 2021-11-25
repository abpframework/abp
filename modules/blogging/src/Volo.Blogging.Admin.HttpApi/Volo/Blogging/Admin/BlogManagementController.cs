using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Admin.Blogs;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Admin
{
    [RemoteService(Name = BloggingAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(BloggingAdminRemoteServiceConsts.ModuleName)]
    [Route("api/blogging/blogs/admin")]
    public class BlogManagementController : AbpControllerBase, IBlogManagementAppService
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
        public async Task<BlogDto> CreateAsync(CreateBlogDto input)
        {
            return await _blogManagementAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
        {
            return await _blogManagementAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _blogManagementAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("clear-cache/{id}")]
        public async Task ClearCacheAsync(Guid id)
        {
            await _blogManagementAppService.ClearCacheAsync(id);
        }
    }
}
