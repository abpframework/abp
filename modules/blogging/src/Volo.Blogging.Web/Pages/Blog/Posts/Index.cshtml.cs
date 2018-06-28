using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class IndexModel : PageModel
    {
        private readonly IPostAppService _postAppService;

        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        public IReadOnlyList<PostWithDetailsDto> Posts { get; set; }

        public IndexModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async Task OnGetAsync()
        {
            var blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Posts = _postAppService.GetListByBlogIdAsync(blog.Id).Items;
        }
    }
}