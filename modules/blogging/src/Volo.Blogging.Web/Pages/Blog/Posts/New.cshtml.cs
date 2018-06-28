using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class NewModel : PageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty]
        public CreatePostViewModel Post { get; set; }

        public BlogDto Blog { get; set; }

        public NewModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async void OnGetAsync()
        {
            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Post = new CreatePostViewModel
            {
                BlogId = Blog.Id
            };
        }

        public async Task<ActionResult> OnPost()
        {
            var blog = await _blogAppService.GetAsync(Post.BlogId);
            var postWithDetailsDto = await _postAppService.CreateAsync(
                new CreatePostDto //TODO: Use automapper
                {
                    BlogId = Post.BlogId,
                    Title = Post.Title,
                    Content = Post.Content
                }
            );

            //TODO: Try Url.Page(...)
            return Redirect(Url.Content($"~/blog/{blog.ShortName}/{postWithDetailsDto.Title}"));
        }

        public class CreatePostViewModel
        {
            [HiddenInput]
            public Guid BlogId { get; set; }

            [Required]
            [StringLength(PostConsts.MaxTitleLength)]
            public string Title { get; set; }

            [StringLength(PostConsts.MaxContentLength)]
            public string Content { get; set; }
        }
    }
}