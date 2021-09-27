using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Blogs
{
    public class UpdateModalModel : CmsKitAdminPageModel
    {
        protected IBlogAdminAppService BlogAdminAppService { get; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public UpdateBlogViewModel ViewModel { get; set; }

        public UpdateModalModel(IBlogAdminAppService blogAdminAppService)
        {
            BlogAdminAppService = blogAdminAppService;
        }

        public async Task OnGetAsync()
        {
            var blog = await BlogAdminAppService.GetAsync(Id);

            ViewModel = ObjectMapper.Map<BlogDto, UpdateBlogViewModel>(blog);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = ObjectMapper.Map<UpdateBlogViewModel, UpdateBlogDto>(ViewModel);

            await BlogAdminAppService.UpdateAsync(Id, dto);

            return NoContent();
        }

        [AutoMap(typeof(BlogDto))]
        [AutoMap(typeof(UpdateBlogDto), ReverseMap = true)]
        public class UpdateBlogViewModel : IHasConcurrencyStamp
        {
            [Required]
            [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxNameLength))]
            public string Name { get; set; }

            [DynamicMaxLength(typeof(BlogConsts), nameof(BlogConsts.MaxSlugLength))]
            [Required]
            public string Slug { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }
        }
    }
}
