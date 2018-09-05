using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;
using Volo.Blogging.Comments;
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
        private readonly ICommentRepository _commentRepository;

        public PostAppService(IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
            _commentRepository = commentRepository;
        }

        public async Task<ListResultDto<PostWithDetailsDto>> GetListByBlogId(Guid id)
        {
            var posts = _postRepository.GetPostsByBlogId(id);

            var postDtos = new List<PostWithDetailsDto>(
                ObjectMapper.Map<List<Post>, List<PostWithDetailsDto>>(posts));

            foreach (var postDto in postDtos)
            {
                var tagIds = (await _postTagRepository.GetListAsync()).Where(pt => pt.PostId == postDto.Id);

                var tags = await _tagRepository.GetListAsync(tagIds.Select(t => t.TagId));

                postDto.Tags = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);

                postDto.CommentCount = (await _commentRepository.GetListAsync()).Count(p => p.PostId == postDto.Id);
            }

            return new ListResultDto<PostWithDetailsDto>(postDtos);
        }

        public async Task<ListResultDto<PostWithDetailsDto>> GetListByBlogIdAndTagName(Guid blogId, string tagName)
        {
            var all = (await GetListByBlogId(blogId)).Items;

            var tag = await _tagRepository.GetByNameAsync(tagName);

            return new ListResultDto<PostWithDetailsDto>(all.Where(p=>p.Tags.Any(t=>t.Id == tag.Id)).ToList());
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

                var tag = await _tagRepository.GetAsync(oldTag.TagId);

                tag.DecreaseUsageCount();
                await _tagRepository.UpdateAsync(tag);
            }

            var tags = await _tagRepository.GetListAsync();

            foreach (var newTag in newTags)
            {
                var tag = tags.FirstOrDefault(t => t.Name == newTag);

                if (tag == null)
                {
                    tag = await _tagRepository.InsertAsync(new Tag(newTag, 1));
                }
                else
                {
                    tag.IncreaseUsageCount();
                    tag = await _tagRepository.UpdateAsync(tag);
                }

                await _postTagRepository.InsertAsync(new PostTag(post.Id, tag.Id));
            }

        }

        private List<string> SplitTags(string tags)
        {
            if (tags.IsNullOrWhiteSpace())
            {
                return new List<string>();
            }
            return new List<string>(tags.Split(",").Select(t=>t.Trim()));
        }
    }
}
