using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [BindProperty]
        public BlogIndexViewModel Blog { get; set; }

        public IndexModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public async Task<IActionResult> OnGetAsync()
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

        public async Task<IActionResult> OnPostAsync()
        {
            await _blogAppService.Create(ObjectMapper.Map<BlogIndexViewModel, CreateBlogDto>(Blog));

            var result = await _blogAppService.GetListAsync();

            if (result.Items.Count == 1)
            {
                var blog = result.Items[0];
                return RedirectToPage("./Posts/Index", new { blogShortName = blog.ShortName});
            }

            Blogs = result.Items;

            return Page();
        }

        public class BlogIndexViewModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string ShortName { get; set; }

            public string Description { get; set; }
        }
    }
}