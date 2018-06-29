using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;

namespace Volo.Blogging.Posts
{
    [Authorize(BloggingPermissions.Posts.Default)]
    public class PostAppService : ApplicationService, IPostAppService
    {
        private readonly IPostRepository _postRepository;

        public PostAppService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public ListResultDto<PostWithDetailsDto> GetListByBlogIdAsync(Guid id)
        {
            var posts = _postRepository.GetPostsByBlogId(id);

            return new ListResultDto<PostWithDetailsDto>(
                ObjectMapper.Map<List<Post>, List<PostWithDetailsDto>>(posts));
        }

        public async Task<PostWithDetailsDto> GetByUrlAsync(GetPostInput input)
        {
            var post = await _postRepository.GetPostByUrl(input.BlogId, input.Url);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }

        public async Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }

        public async Task<GetPostForEditOutput> GetForEditAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);

            var dto = new GetPostForEditOutput
            {
                Id = post.Id,
                BlogId = post.BlogId,
                Content = post.Content,
                Title = post.Title,
                Url = post.Url
            };

            return dto;
        }

        [Authorize(BloggingPermissions.Posts.Update)]
        public async Task<PostWithDetailsDto> UpdateAsync(Guid id, UpdatePostDto input)
        {
            var post = await _postRepository.GetAsync(id);

            post.SetTitle(input.Title);
            post.SetUrl(input.Url);
            post.Content = input.Content;

            post = await _postRepository.UpdateAsync(post);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }

        [Authorize(BloggingPermissions.Posts.Create)]
        public async Task<PostWithDetailsDto> CreateAsync(CreatePostDto input)
        {
            var post = new Post(
                id: GuidGenerator.Create(),
                blogId: input.BlogId,
                creatorId: CurrentUser.GetId(),
                title: input.Title,
                url: input.Url
            ) {Content = input.Content};

            await _postRepository.InsertAsync(post);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }
    }
}
