using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs
{
    public class CreateModalModel : CmsKitAdminPageModel
    {
        protected IBlogAdminAppService BlogAdminAppService { get; }

        [BindProperty]
        public CreateBlogViewModel ViewModel { get; set; }

        public CreateModalModel(IBlogAdminAppService blogAdminAppService)
        {
            BlogAdminAppService = blogAdminAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = ObjectMapper.Map<CreateBlogViewModel, CreateBlogDto>(ViewModel);

            await BlogAdminAppService.CreateAsync(dto);

            return NoContent();
        }

        [AutoMap(typeof(CreateBlogDto), ReverseMap = true)]
        public class CreateBlogViewModel
        {
            [Required]
            [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
            public string Name { get; set; }

            [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxSlugLength))]
            [Required]
            public string Slug { get; set; }
        }
    }
}
