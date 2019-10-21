using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class NewModel : BloggingPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly IAuthorizationService _authorization;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty]
        public CreatePostViewModel Post { get; set; }

        public BlogDto Blog { get; set; }

        public NewModel(IPostAppService postAppService, IBlogAppService blogAppService, IAuthorizationService authorization)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _authorization = authorization;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Posts.Create))
            {
                return Redirect("/");
            }

            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Post = new CreatePostViewModel
            {
                BlogId = Blog.Id
            };

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            var blog = await _blogAppService.GetAsync(Post.BlogId);
            var postWithDetailsDto = await _postAppService.CreateAsync(ObjectMapper.Map<CreatePostViewModel,CreatePostDto>(Post));

            //TODO: Try Url.Page(...)
            return Redirect(Url.Content($"~/blog/{WebUtility.UrlEncode(blog.ShortName)}/{WebUtility.UrlEncode(postWithDetailsDto.Url)}"));
        }

        public class CreatePostViewModel
        {
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
}