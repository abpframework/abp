using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Volo.Blogging.Blogs;
using Volo.Blogging.Pages.Blogs.Shared.Helpers;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blogs.Posts
{
    public class EditModel : BloggingPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly IAuthorizationService _authorization;
        private readonly BloggingUrlOptions _blogOptions;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PostId { get; set; }

        [BindProperty]
        public EditPostViewModel Post { get; set; }

        public EditModel(IPostAppService postAppService, IBlogAppService blogAppService, IAuthorizationService authorization, IOptions<BloggingUrlOptions> blogOptions)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _authorization = authorization;
            _blogOptions = blogOptions.Value;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Posts.Update))
            {
                return Redirect("/");
            }

            if (_blogOptions.SingleBlogMode.Enabled)
            {
                BlogShortName = _blogOptions.SingleBlogMode.BlogName;
            }
            
            if (BlogNameControlHelper.IsProhibitedFileFormatName(BlogShortName))
            {
                return NotFound();
            }

            var postDto = await _postAppService.GetAsync(new Guid(PostId));
            Post = ObjectMapper.Map<PostWithDetailsDto, EditPostViewModel>(postDto);
            Post.Tags = String.Join(", ", postDto.Tags.Select(p => p.Name).ToArray());
            Post.Url = WebUtility.UrlDecode(Post.Url);

            return Page();
        }

        public virtual async Task<ActionResult> OnPostAsync()
        {
            var post = new UpdatePostDto
            {
                BlogId = Post.BlogId,
                Title = Post.Title,
                Url = WebUtility.UrlEncode(Post.Url),
                CoverImage = Post.CoverImage,
                Content = Post.Content,
                Tags = Post.Tags,
                Description = Post.Description.IsNullOrEmpty() ?
                    Post.Content.Truncate(PostConsts.MaxSeoFriendlyDescriptionLength) :
                    Post.Description
            };

            var editedPost = await _postAppService.UpdateAsync(Post.Id, post);
            var blog = await _blogAppService.GetAsync(editedPost.BlogId);

            Dictionary<string, object> routeValues = new()
            {
                { nameof(DetailModel.PostUrl), editedPost.Url }
            };

            if (!_blogOptions.SingleBlogMode.Enabled)
            {
                routeValues.Add(nameof(DetailModel.BlogShortName), blog.ShortName);
            }
            return RedirectToPage("/Blogs/Posts/Detail", routeValues);
        }
    }

    public class EditPostViewModel : IHasConcurrencyStamp

    {
        [Required]
        [HiddenInput]
        public Guid Id { get; set; }

        [Required]
        [HiddenInput]
        public Guid BlogId { get; set; }

        [Required]
        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [HiddenInput]
        public string CoverImage { get; set; }

        [Required]
        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxUrlLength))]
        public string Url { get; set; }

        [Required]
        [HiddenInput]
        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxContentLength))]
        public string Content { get; set; }

        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxDescriptionLength))]
        public string Description { get; set; }

        public string Tags { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }
    }
}
