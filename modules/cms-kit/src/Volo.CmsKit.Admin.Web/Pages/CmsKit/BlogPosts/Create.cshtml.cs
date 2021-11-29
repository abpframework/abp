using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts
{
    public class CreateModel : CmsKitAdminPageModel
    {
        protected IBlogPostAdminAppService BlogPostAdminAppService { get; }

        [BindProperty]
        public CreateBlogPostViewModel ViewModel { get; set; }

        public CreateModel(
            IBlogPostAdminAppService blogPostAdminAppService)
        {
            BlogPostAdminAppService = blogPostAdminAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = ObjectMapper.Map<CreateBlogPostViewModel, CreateBlogPostDto>(ViewModel);

            var created = await BlogPostAdminAppService.CreateAsync(dto);

            return new OkObjectResult(created);
        }

        [AutoMap(typeof(CreateBlogPostDto), ReverseMap = true)]
        public class CreateBlogPostViewModel
        {
            [Required]
            [DynamicFormIgnore]
            public Guid BlogId { get; set; }

            [Required]
            [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxTitleLength))]
            [DynamicFormIgnore]
            public string Title { get; set; }

            [Required]
            [DynamicStringLength(
                typeof(BlogPostConsts),
                nameof(BlogPostConsts.MaxSlugLength),
                nameof(BlogPostConsts.MinSlugLength))]
            [DynamicFormIgnore]
            public string Slug { get; set; }

            [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxShortDescriptionLength))]
            public string ShortDescription { get; set; }
            
            [HiddenInput]
            [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxContentLength))]
            public string Content { get; set; }

            [HiddenInput]
            public Guid? CoverImageMediaId { get; set; }
        }
    }
}
