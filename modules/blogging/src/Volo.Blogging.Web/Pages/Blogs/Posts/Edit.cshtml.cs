using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
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

        public async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Posts.Update))
            {
                return Redirect("/");
            }

            var postDto = await _postAppService.GetAsync(new Guid(PostId));
            Post = ObjectMapper.Map<PostWithDetailsDto, EditPostViewModel>(postDto);
            Post.Tags = String.Join(", ", postDto.Tags.Select(p=>p.Name).ToArray());

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            var post = new UpdatePostDto
            {
                BlogId = Post.BlogId,
                Title = Post.Title,
                Url = Post.Url,
                CoverImage = Post.CoverImage,
                Content = Post.Content,
                Tags = Post.Tags
            };

            var editedPost = await _postAppService.UpdateAsync(Post.Id, post);
            var blog = await _blogAppService.GetAsync(editedPost.BlogId);

           // return Redirect(Url.Content($"~/blog/{WebUtility.UrlEncode(blog.ShortName)}/{WebUtility.UrlEncode(editedPost.Url)}"));
            return RedirectToPage("/Blog/Posts/Detail", new { blogShortName = blog.ShortName, postUrl = editedPost.Url });
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
        [StringLength(PostConsts.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [HiddenInput]
        public string CoverImage { get; set; }

        [Required]
        [StringLength(PostConsts.MaxUrlLength)]
        public string Url { get; set; }

        [HiddenInput]
        [StringLength(PostConsts.MaxContentLength)]
        public string Content { get; set; }

        public string Tags { get; set; }
    }
}