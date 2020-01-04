using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Volo.Blogging.Comments;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;
using Volo.Blogging.Users;

namespace Volo.Blogging.Posts
{
    public class PostAppService : BloggingAppServiceBase, IPostAppService
    {
        protected IBlogUserLookupService UserLookupService { get; }

        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;

        public PostAppService(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository, IBlogUserLookupService userLookupService)
        {
            UserLookupService = userLookupService;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }

        public async Task<ListResultDto<PostWithDetailsDto>> GetListByBlogIdAndTagName(Guid id, string tagName)
        {
            var posts = await _postRepository.GetPostsByBlogId(id);
            var tag = tagName.IsNullOrWhiteSpace() ? null : await _tagRepository.FindByNameAsync(id, tagName);
            var userDictionary = new Dictionary<Guid, BlogUserDto>();
            var postDtos = new List<PostWithDetailsDto>(ObjectMapper.Map<List<Post>, List<PostWithDetailsDto>>(posts));

            foreach (var postDto in postDtos)
            {
                postDto.Tags = await GetTagsOfPost(postDto.Id);
            }

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
                    if (!userDictionary.ContainsKey(postDto.CreatorId.Value))
                    {
                        var creatorUser = await UserLookupService.FindByIdAsync(postDto.CreatorId.Value);
                        if (creatorUser != null)
                        {
                            userDictionary[creatorUser.Id] = ObjectMapper.Map<BlogUser, BlogUserDto>(creatorUser);
                        }
                    }

                    if (userDictionary.ContainsKey(postDto.CreatorId.Value))
                    {
                        postDto.Writer = userDictionary[(Guid)postDto.CreatorId];
                    }
                }
            }

            return new ListResultDto<PostWithDetailsDto>(postDtos);
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

        [Authorize(BloggingPermissions.Posts.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);

            await AuthorizationService.CheckAsync(post, CommonOperations.Delete);

            var tags = await GetTagsOfPost(id);
            await _tagRepository.DecreaseUsageCountOfTagsAsync(tags.Select(t => t.Id).ToList());
            await _commentRepository.DeleteOfPost(id);

            await _postRepository.DeleteAsync(id);
        }

        [Authorize(BloggingPermissions.Posts.Update)]
        public async Task<PostWithDetailsDto> UpdateAsync(Guid id, UpdatePostDto input)
        {
            var post = await _postRepository.GetAsync(id);

            input.Url = await RenameUrlIfItAlreadyExistAsync(input.BlogId, input.Url, post);

            await AuthorizationService.CheckAsync(post, CommonOperations.Update);

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
            input.Url = await RenameUrlIfItAlreadyExistAsync(input.BlogId, input.Url);

            var post = new Post(
                id: GuidGenerator.Create(),
                blogId: input.BlogId,
                title: input.Title,
                coverImage: input.CoverImage,
                url: input.Url
            )
            { Content = input.Content };

            await _postRepository.InsertAsync(post);

            var tagList = SplitTags(input.Tags);
            await SaveTags(tagList, post);

            return ObjectMapper.Map<Post, PostWithDetailsDto>(post);
        }

        private async Task<string> RenameUrlIfItAlreadyExistAsync(Guid blogId, string url, Post existingPost = null)
        {
            var postList = await _postRepository.GetListAsync();

            if (postList.Where(p => p.Url == url).WhereIf(existingPost != null, p =>  existingPost.Id != p.Id).Any())
            {
                return url + "-" + Guid.NewGuid().ToString().Substring(0, 5);
            }

            return url;
        }

        private async Task SaveTags(ICollection<string> newTags, Post post)
        {
            await RemoveOldTags(newTags, post);

            await AddNewTags(newTags, post);
        }

        private async Task RemoveOldTags(ICollection<string> newTags, Post post)
        {
            foreach (var oldTag in post.Tags)
            {
                var tag = await _tagRepository.GetAsync(oldTag.TagId);

                var oldTagNameInNewTags = newTags.FirstOrDefault(t => t == tag.Name);

                if (oldTagNameInNewTags == null)
                {
                    post.RemoveTag(oldTag.TagId);

                    tag.DecreaseUsageCount();
                    await _tagRepository.UpdateAsync(tag);
                }
                else
                {
                    newTags.Remove(oldTagNameInNewTags);
                }
            }
        }

        private async Task AddNewTags(IEnumerable<string> newTags, Post post)
        {
            var tags = await _tagRepository.GetListAsync(post.BlogId);

            foreach (var newTag in newTags)
            {
                var tag = tags.FirstOrDefault(t => t.Name == newTag);

                if (tag == null)
                {
                    tag = await _tagRepository.InsertAsync(new Tag(GuidGenerator.Create(), post.BlogId, newTag, 1));
                }
                else
                {
                    tag.IncreaseUsageCount();
                    tag = await _tagRepository.UpdateAsync(tag);
                }

                post.AddTag(tag.Id);
            }
        }

        private async Task<List<TagDto>> GetTagsOfPost(Guid id)
        {
            var tagIds = (await _postRepository.GetAsync(id)).Tags;

            var tags = await _tagRepository.GetListAsync(tagIds.Select(t => t.TagId));

            return ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
        }

        private List<string> SplitTags(string tags)
        {
            if (tags.IsNullOrWhiteSpace())
            {
                return new List<string>();
            }
            return new List<string>(tags.Split(",").Select(t => t.Trim()));
        }

        private Task<List<PostWithDetailsDto>> FilterPostsByTag(IEnumerable<PostWithDetailsDto> allPostDtos, Tag tag)
        {
            var filteredPostDtos = allPostDtos.Where(p => p.Tags?.Any(t => t.Id == tag.Id) ?? false).ToList();
           
            return Task.FromResult(filteredPostDtos);
        }
    }
}
