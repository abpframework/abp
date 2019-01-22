using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    [RemoteService]
    [Area("blogging")]
    [Controller]
    [ControllerName("Posts")]
    [Route("api/blogging/posts")]
    [DisableAuditing]
    public class PostsController : AbpController, IPostAppService
    {
        private readonly IPostAppService _postAppService;

        public PostsController(IPostAppService postAppService)
        {
            _postAppService = postAppService;
        }

        [HttpGet]
        [Route("{blogId}/all")]
        public Task<ListResultDto<PostWithDetailsDto>> GetListByBlogIdAndTagName(Guid blogId, string tagName)
        {
            return _postAppService.GetListByBlogIdAndTagName(blogId, tagName);
        }

        [HttpGet]
        [Route("read/{id}")]
        public Task<PostWithDetailsDto> GetForReadingAsync(GetPostInput input)
        {
            return _postAppService.GetForReadingAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            return _postAppService.GetAsync(id);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _postAppService.DeleteAsync(id);
        }

        [HttpPost]
        public Task<PostWithDetailsDto> CreateAsync(CreatePostDto input)
        {
            return _postAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<PostWithDetailsDto> UpdateAsync(Guid id, UpdatePostDto input)
        {
            return _postAppService.UpdateAsync(id, input);
        }
    }
}
