using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;
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

        public IReadOnlyList<PostDto> Posts { get; set; }

        public IndexModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async Task OnGet()
        {
            var blog = await _blogAppService.GetByShortNameAsync(BlogShortName);

            Posts = _postAppService.GetPostsByBlogId(blog.Id).Items;
        }
    }
}