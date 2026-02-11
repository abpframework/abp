using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging
{
    [RemoteService(Name = BloggingRemoteServiceConsts.RemoteServiceName)]
    [Area(BloggingRemoteServiceConsts.ModuleName)]
    [Route("api/blogging/blogs")]
    public class BlogsController : AbpControllerBase, IBlogAppService
    {
        private readonly IBlogAppService _blogAppService;

        public BlogsController(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        [HttpGet]
        public virtual async Task<ListResultDto<BlogDto>> GetListAsync()
        {
            return await _blogAppService.GetListAsync();
        }

        [HttpGet]
        [Route("by-shortname/{shortName}")]
        public virtual async Task<BlogDto> GetByShortNameAsync(string shortName)
        {
            return await _blogAppService.GetByShortNameAsync(shortName);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<BlogDto> GetAsync(Guid id)
        {
            return await _blogAppService.GetAsync(id);
        }
    }
}
