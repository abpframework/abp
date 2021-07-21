using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Validation;
using Volo.Blogging.Blogs;
using Volo.Blogging.Pages.Blogs.Shared.Helpers;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class EditModel : BloggingPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly IAuthorizationService _authorization;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PostId { get; set; }

        [BindProperty]
        public EditPostViewModel Post { get; set; }

        public EditModel(IPostAppService postAppService, IBlogAppService blogAppService, IAuthorizationService authorization)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _authorization = authorization;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Posts.Update))
            {
                return Redirect("/");
            }
            if (BlogNameControlHelper.IsProhibitedFileFormatName(BlogShortName))
            {
                return NotFound();
            }

            var postDto = await _postAppService.GetAsync(new Guid(PostId));
            Post = ObjectMapper.Map<PostWithDetailsDto, EditPostViewModel>(postDto);
            Post.Tags = String.Join(", ", postDto.Tags.Select(p => p.Name).ToArray());

            return Page();
        }

        public virtual async Task<ActionResult> OnPostAsync()
        {
            var post = new UpdatePostDto
            {
                BlogId = Post.BlogId,
                Title = Post.Title,
                Url = Post.Url,
                CoverImage = Post.CoverImage,
                Content = Post.Content,
                Tags = Post.Tags,
                Description = Post.Description.IsNullOrEmpty() ?
                    Post.Content.Truncate(PostConsts.MaxSeoFriendlyDescriptionLength) :
                    Post.Description
            };

            var editedPost = await _postAppService.UpdateAsync(Post.Id, post);
            var blog = await _blogAppService.GetAsync(editedPost.BlogId);

            return RedirectToPage("/Blogs/Posts/Detail", new { blogShortName = blog.ShortName, postUrl = editedPost.Url });
        }
    }

    public class EditPostViewModel
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
    }
}
