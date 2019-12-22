using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    [RemoteService]
    [Area("blogging")]
    [Route("api/blogging/posts")]
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
        [Route("read")]
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

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _postAppService.DeleteAsync(id);
        }
    }
}
