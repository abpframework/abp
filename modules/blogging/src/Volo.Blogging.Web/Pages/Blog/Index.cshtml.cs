using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;

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

        public async Task<IActionResult> OnGet()
        {
            var result = await _blogAppService.GetListAsync();

            Blogs = result.Items;
            return Page();
        }
    }
}