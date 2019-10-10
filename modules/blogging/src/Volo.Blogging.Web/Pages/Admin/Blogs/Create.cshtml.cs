using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Pages.Blog;

namespace Volo.Blogging.Pages.Admin.Blogs
{
    public class CreateModel : BloggingPageModel
    {
        private readonly IBlogAppService _blogAppService;
        private readonly IAuthorizationService _authorization;

        [BindProperty]
        public BlogCreateModalView Blog { get; set; } = new BlogCreateModalView();

        public CreateModel(IBlogAppService blogAppService, IAuthorizationService authorization)
        {
            _blogAppService = blogAppService;
            _authorization = authorization;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Blogs.Create))
            {
                return Redirect("/");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var blogDto = ObjectMapper.Map<BlogCreateModalView, CreateBlogDto>(Blog);

            await _blogAppService.Create(blogDto);

            return NoContent();
        }


        public class BlogCreateModalView
        {
            [Required]
            [StringLength(BlogConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [StringLength(BlogConsts.MaxShortNameLength)]
            public string ShortName { get; set; }

            [StringLength(BlogConsts.MaxDescriptionLength)]
            public string Description { get; set; }

        }
    }
}