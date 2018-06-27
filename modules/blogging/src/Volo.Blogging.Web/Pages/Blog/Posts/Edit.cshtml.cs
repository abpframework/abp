using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class EditModel : PageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PostId { get; set; }

        public PostWithDetailsDto Post { get; set; }

        public BlogDto Blog { get; set; }

        public EditModel(IPostAppService postAppService, IBlogAppService blogAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
        }

        public async void OnGet()
        {
            var blog = await _blogAppService.GetByShortNameAsync(BlogShortName);

            Post = await _postAppService.GetAsync(new Guid(PostId));

            Blog = blog;
        }

        public async Task<ActionResult> OnPost(Guid id, UpdatePostDto post)
        {
            var editedPost = await _postAppService.UpdateAsync(id, post);
            var blog = await _blogAppService.GetAsync(editedPost.BlogId);

            return Redirect(Url.Content($"~/blog/{blog.ShortName}/{editedPost.Title}"));
        }
    }
}