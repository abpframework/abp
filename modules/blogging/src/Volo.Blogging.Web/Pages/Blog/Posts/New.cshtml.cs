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

        public CreatePostDto Post { get; set; }

        public BlogDto Blog { get; set; }

        public NewModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async void OnGet()
        {
            var blog = await _blogAppService.GetByShortNameAsync(BlogShortName);

            Post = new CreatePostDto()
            {
                BlogId = blog.Id
            };

            Blog = blog;
        }

        public async Task<ActionResult> OnPost(CreatePostDto post)
        {
            var insertedPost = await _postAppService.CreateAsync(post);
            var blog = await _blogAppService.GetAsync(insertedPost.BlogId);

            return Redirect(Url.Content($"~/blog/{blog.ShortName}/{insertedPost.Title}"));
        }
    }
}