using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Validation;
using Volo.Blogging.Admin.Blogs;
using Volo.Blogging.Blogs;

namespace Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs
{
    public class CreateModel : BloggingAdminPageModel
    {
        private readonly IBlogManagementAppService _blogAppService;
        private readonly IAuthorizationService _authorization;

        [BindProperty]
        public BlogCreateModalView Blog { get; set; } = new BlogCreateModalView();

        public CreateModel(IBlogManagementAppService blogAppService, IAuthorizationService authorization)
        {
            _blogAppService = blogAppService;
            _authorization = authorization;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Blogs.Create))
            {
                return Redirect("/");
            }

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var blogDto = ObjectMapper.Map<BlogCreateModalView, CreateBlogDto>(Blog);

            await _blogAppService.CreateAsync(blogDto);

            return NoContent();
        }


        public class BlogCreateModalView
        {
            [Required]
            [DynamicStringLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
            public string Name { get; set; }

            [Required]
            [DynamicStringLength(typeof(BlogConsts), nameof(BlogConsts.MaxShortNameLength))]
            public string ShortName { get; set; }

            [DynamicStringLength(typeof(BlogConsts), nameof(BlogConsts.MaxDescriptionLength))]
            public string Description { get; set; }

        }
    }
}
