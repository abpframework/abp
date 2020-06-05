using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Pages.Blog
{
    public class IndexModel : AbpPageModel
    {
        private readonly IBlogAppService _blogAppService;

        public IReadOnlyList<BlogDto> Blogs { get; private set; }

        public IndexModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var result = await _blogAppService.GetListAsync();

            if (result.Items.Count == 1)
            {
                var blog = result.Items[0];
                return RedirectToPage("./Posts/Index", new { blogShortName = blog.ShortName });
            }

            Blogs = result.Items;

            return Page();
        }
    }
}
