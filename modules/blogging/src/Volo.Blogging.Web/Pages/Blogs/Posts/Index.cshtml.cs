using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Pages.Blogs.Shared.Helpers;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Pages.Blog.Posts
{
    public class IndexModel : BloggingPageModel
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;
        private readonly ITagAppService _tagAppService;

        [BindProperty(SupportsGet = true)]
        public string BlogShortName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TagName { get; set; }

        public BlogDto Blog { get; set; }

        public IReadOnlyList<PostWithDetailsDto> Posts { get; set; }

        public IReadOnlyList<TagDto> PopularTags { get; set; }

        public IndexModel(IPostAppService postAppService, IBlogAppService blogAppService, ITagAppService tagAppService)
        {
            _postAppService = postAppService;
            _blogAppService = blogAppService;
            _tagAppService = tagAppService;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (BlogNameControlHelper.IsProhibitedFileFormatName(BlogShortName))
            {
                return NotFound();
            }

            Blog = await _blogAppService.GetByShortNameAsync(BlogShortName);
            Posts = (await _postAppService.GetListByBlogIdAndTagNameAsync(Blog.Id, TagName)).Items;
            PopularTags = (await _tagAppService.GetPopularTagsAsync(Blog.Id, new GetPopularTagsInput {ResultCount = 10, MinimumPostCount = 2}));

            return Page();
        }
    }
}