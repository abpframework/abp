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
using Volo.Blogging.Users;

namespace Volo.Blogging.Posts
{
    /* TODO: Custom policy with configuration.
     * We should create a custom policy to see the blog as read only if the blog is
     * configured as 'public' or the current user has the related permission.
     */
    //[Authorize(BloggingPermissions.Posts.Default)]
    public class PostAppService : ApplicationService, IPostAppService
    {
        protected IBlogUserLookupService UserLookupService { get; }

        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly ICommentRepository _commentRepository;

        public PostAppService(IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository, ICommentRepository commentRepository, IBlogUserLookupService userLookupService)
        {
            UserLookupService = userLookupService;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
            _commentRepository = commentRepository;
        }

        public async Task<ListResultDto<PostWithDetailsDto>> GetListByBlogIdAndTagName(Guid id, string tagName)
        {
            var posts = _postRepository.GetPostsByBlogId(id);
            var tag = tagName.IsNullOrWhiteSpace()? null: await _tagRepository.GetByNameAsync(tagName);
            var userDictionary = new Dictionary<Guid, BlogUserDto>();
            var postDtos = new List<PostWithDetailsDto>(ObjectMapper.Map<List<Post>, List<PostWithDetailsDto>>(posts));

            if (tag != null)
            {
                postDtos = await FilterPostsByTag(postDtos, tag);
            }

            foreach (var postDto in postDtos)
            {
                postDto.CommentCount = await _commentRepository.GetCommentCountOfPostAsync(postDto.Id);
            }

            foreach (var postDto in postDtos)
            {
                if (postDto.CreatorId.HasValue)
                {
                    var creatorUser = await UserLookupService.FindByIdAsync(postDto.CreatorId.Value);

                    if (creatorUser != null && !userDictionary.ContainsKey(creatorUser.Id))
                    {
                        userDictionary.Add(creatorUser.Id, ObjectMapper.Map<BlogUser, BlogUserDto>(creatorUser));
                    }
                }
            }

            foreach (var postDto in postDtos)
            {
                if (postDto.CreatorId.HasValue && userDictionary.ContainsKey((Guid) postDto.CreatorId))
                {
                    postDto.Writer = userDictionary[(Guid)postDto.CreatorId];
                }
            }

            return new ListResultDto<PostWithDetailsDto>(postDtos);
        }

        private async Task<List<PostWithDetailsDto>> FilterPostsByTag(List<PostWithDetailsDto> allPostDtos, Tag tag)
        {
            var filteredPostDtos = new List<PostWithDetailsDto>();

            foreach (var postDto in allPostDtos)
            {
                if (await _postTagRepository.FindByTagIdAndPostIdAsync(postDto.Id, tag.Id) == null)
                {
                    continue;
                }

                filteredPostDtos.Add(postDto);
            }

            return filteredPostDtos;
        }

        public async Task<PostWithDetailsDto> GetForReadingAsync(GetPostInput input)
        {
            var post = await _postRepository.GetPostByUrl(input.BlogId, input.Url);

            post.IncreaseReadCount();

            var postDto = ObjectMapper.Map<Post, PostWithDetailsDto>(post);

            postDto.Tags = await GetTagsOfPost(postDto.Id);

            if (postDto.CreatorId.HasValue)
            {
                var creatorUser = await UserLookupService.FindByIdAsync(postDto.CreatorId.Value);

                postDto.Writer = ObjectMapper.Map<BlogUser, BlogUserDto>(creatorUser);
            }

            return postDto;
        }

        public async Task<PostWithDetailsDto> GetAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);

            var postDto = ObjectMapper.Map<Post, PostWithDetailsDto>(post);

            postDto.Tags = await GetTagsOfPost(postDto.Id);

            if (postDto.CreatorId.HasValue)
            {
                var creatorUser = await UserLookupService.FindByIdAsync(postDto.CreatorId.Value);

                postDto.Writer = ObjectMapper.Map<BlogUser, BlogUserDto>(creatorUser);
            }

            return postDto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var tags = await GetTagsOfPost(id);
            _tagRepository.DecreaseUsageCountOfTags(tags.Select(t=>t.Id).ToList());
            _postTagRepository.DeleteOfPost(id);
            _commentRepository.DeleteOfPost(id);

            await _postRepository.DeleteAsync(id);
        }

        [Authorize(BloggingPermissions.Posts.Update)]
        public async Task<PostWithDetailsDto> UpdateAsync(Guid id, UpdatePostDto input)
        {
            var post = await _postRepository.GetAsync(id);

            post.SetTitle(input.Title);
            post.SetUrl(input.Url);
            post.Content = input.Content;
            post.CoverImage = input.CoverImage;

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
                coverImage: input.CoverImage,
                url: input.Url
            ) {Content = input.Content};

            await _postRepository.InsertAsync(post);

            var tagList = SplitTags(input.Tags);
            await SaveTags(tagList, post);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }

        private async Task SaveTags(List<String> newTags, Post post)
        {

            await RemoveOldTags(newTags, post);

            await AddNewTags(newTags, post);
        }

        private async Task RemoveOldTags(List<string> newTags, Post post)
        {
            var oldTags = (await _postTagRepository.GetListAsync()).Where(pt => pt.PostId == post.Id).ToList();

            foreach (var oldTag in oldTags)
            {
                var tag = await _tagRepository.GetAsync(oldTag.TagId);

                var oldTagNameInNewTags = newTags.FirstOrDefault(t => t == tag.Name);

                if (oldTagNameInNewTags == null)
                {
                    await _postTagRepository.DeleteAsync(oldTag);

                    tag.DecreaseUsageCount();
                    await _tagRepository.UpdateAsync(tag);
                }
                else
                {
                    newTags.Remove(oldTagNameInNewTags);
                }
            }
        }

        private async Task AddNewTags(List<string> newTags, Post post)
        {
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

        private async Task<List<TagDto>> GetTagsOfPost(Guid id)
        {
            var tagIds = (await _postTagRepository.GetListAsync()).Where(pt => pt.PostId == id);

            var tags = await _tagRepository.GetListAsync(tagIds.Select(t => t.TagId));

            return ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
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
