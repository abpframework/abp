using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Blogging.Blogs;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Posts
{
    [Authorize(BloggingPermissions.Posts.Default)]
    public class PostAppService : ApplicationService, IPostAppService
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostTagRepository _postTagRepository;

        public PostAppService(IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
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

            var postDto = ObjectMapper.Map<Post, PostWithDetailsDto>(post);

            var tagIds = (await _postTagRepository.GetListAsync()).Where(pt => pt.PostId == postDto.Id);

            var tags = await _tagRepository.GetListAsync(tagIds.Select(t => t.TagId));

            postDto.Tags = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);

            return postDto;
        }

        public async Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);

            var postDto = ObjectMapper.Map<Post, PostWithDetailsDto>(post);

            var tagIds = (await _postTagRepository.GetListAsync()).Where(pt => pt.PostId == postDto.Id);

            var tags = await _tagRepository.GetListAsync(tagIds.Select(t => t.TagId));

            postDto.Tags = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);

            return postDto;
        }

        [Authorize(BloggingPermissions.Posts.Update)]
        public async Task<PostWithDetailsDto> UpdateAsync(Guid id, UpdatePostDto input)
        {
            var post = await _postRepository.GetAsync(id);

            post.SetTitle(input.Title);
            post.SetUrl(input.Url);
            post.Content = input.Content;

            post = await _postRepository.UpdateAsync(post);

            var tagList = SplitTags(input.Tags);
            await SaveTags(tagList, post);

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

            var tagList = SplitTags(input.Tags);
            await SaveTags(tagList, post);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }

        private async Task SaveTags(List<String> newTags, Post post)
        {
            var oldTags = (await _postTagRepository.GetListAsync()).Where(pt => pt.PostId == post.Id).ToList();

            foreach (var oldTag in oldTags)
            {
                await _postTagRepository.DeleteAsync(oldTag);
            }

            var tags = await _tagRepository.GetListAsync();

            foreach (var newTag in newTags)
            {
                var tag = tags.FirstOrDefault(t => t.Name == newTag);

                if (tag == null)
                {
                    tag = await _tagRepository.InsertAsync(new Tag(newTag));
                }

                await _postTagRepository.InsertAsync(new PostTag(post.Id, tag.Id));
            }

        }

        private List<string> SplitTags(string tags)
        {
            return new List<string>(tags.Split(",").Select(t=>t.Trim()));
        }
    }
}
