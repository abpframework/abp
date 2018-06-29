using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class DetailModel : PageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PostUrl { get; set; }

        public PostWithDetailsDto Post { get; set; }

        public BlogDto Blog { get; set; }

        public DetailModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async void OnGet()
        {
            var blog = await _blogAppService.GetByShortNameAsync(BlogShortName);

            Post = await _postAppService.GetByUrlAsync(new GetPostInput {BlogId = blog.Id , Url = PostUrl});
            Blog = blog;
        }
    }
}