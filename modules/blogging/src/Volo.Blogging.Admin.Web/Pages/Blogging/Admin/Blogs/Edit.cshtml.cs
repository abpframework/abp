using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Volo.Blogging.Admin.Blogs;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs
{
    public class EditModel : BloggingAdminPageModel
    {
        private readonly IBlogManagementAppService _blogAppService;
        private readonly IAuthorizationService _authorization;

        [BindProperty(SupportsGet = true)]
        public Guid BlogId { get; set; }

        [BindProperty]
        public BlogEditViewModel Blog { get; set; } = new BlogEditViewModel();

        public EditModel(IBlogManagementAppService blogAppService, IAuthorizationService authorization)
        {
            _blogAppService = blogAppService;
            _authorization = authorization;
        }

        public virtual async Task<ActionResult> OnGetAsync()
        {
            if (!await _authorization.IsGrantedAsync(BloggingPermissions.Blogs.Update))
            {
                return Redirect("/");
            }

            var blog = await _blogAppService.GetAsync(BlogId);

            Blog = ObjectMapper.Map<BlogDto, BlogEditViewModel>(blog);

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            await _blogAppService.UpdateAsync(Blog.Id, new UpdateBlogDto()
            {
                Name = Blog.Name,
                ShortName = Blog.ShortName,
                Description = Blog.Description
            });

            return Page();
        }

        public class BlogEditViewModel : IHasConcurrencyStamp
        {
            [HiddenInput]
            [Required]
            public Guid Id { get; set; }

            [Required]
            [DynamicStringLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
            public string Name { get; set; }

            [Required]
            [DynamicStringLength(typeof(BlogConsts), nameof(BlogConsts.MaxShortNameLength))]
            public string ShortName { get; set; }

            [DynamicStringLength(typeof(BlogConsts), nameof(BlogConsts.MaxDescriptionLength))]
            public string Description { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }
        }
    }
}
