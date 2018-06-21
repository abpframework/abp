using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Collections.Generic;

namespace Volo.Blogging.Posts
{
    public class PostAppService : ApplicationService, IPostAppService
    {
        private readonly IPostRepository _postRepository;

        public PostAppService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public ListResultDto<PostDto> GetPostsByBlogId(Guid id)
        {
            var posts = _postRepository.GetPostsByBlogId(id);

            return new ListResultDto<PostDto>(
                ObjectMapper.Map<List<Post>, List<PostDto>>(posts));
        }

        public async Task<PostDto> GetPost(GetPostInput input)
        {
            var post = await _postRepository.GetPost(input.BlogId, input.Title);

            return  ObjectMapper.Map<Post,PostDto>(post);
        }
    }
}
