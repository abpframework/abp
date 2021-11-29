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
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.BlogPosts
{
    public class UpdateModel : CmsKitAdminPageModel
    {
        protected IBlogPostAdminAppService BlogPostAdminAppService { get; }
        protected IBlogFeatureAppService BlogFeatureAppService { get; }

        [BindProperty]
        public virtual UpdateBlogPostViewModel ViewModel { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public virtual Guid Id { get; set; }

        public virtual BlogFeatureDto TagsFeature { get; protected set; }

        public UpdateModel(
            IBlogPostAdminAppService blogPostAdminAppService,
            IBlogFeatureAppService blogFeatureAppService)
        {
            BlogPostAdminAppService = blogPostAdminAppService;
            BlogFeatureAppService = blogFeatureAppService;
        }

        public virtual async Task OnGetAsync()
        {
            var blogPost = await BlogPostAdminAppService.GetAsync(Id);

            ViewModel = ObjectMapper.Map<BlogPostDto, UpdateBlogPostViewModel>(blogPost);

            TagsFeature = await BlogFeatureAppService.GetOrDefaultAsync(blogPost.BlogId, GlobalFeatures.TagsFeature.Name);
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var dto = ObjectMapper.Map<UpdateBlogPostViewModel, UpdateBlogPostDto>(ViewModel);

            await BlogPostAdminAppService.UpdateAsync(Id, dto);

            return NoContent();
        }

        [AutoMap(typeof(BlogPostDto))]
        [AutoMap(typeof(UpdateBlogPostDto), ReverseMap = true)]
        public class UpdateBlogPostViewModel : IHasConcurrencyStamp
        {
            [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxTitleLength))]
            [Required]
            [DynamicFormIgnore]
            public string Title { get; set; }

            [DynamicStringLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxSlugLength), nameof(BlogPostConsts.MinSlugLength))]
            [Required]
            [DisplayOrder(10000)]
            [DynamicFormIgnore]
            public string Slug { get; set; }

            [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxShortDescriptionLength))]
            [DisplayOrder(10001)]
            public string ShortDescription { get; set; }
            
            [HiddenInput]
            [DynamicMaxLength(typeof(BlogPostConsts), nameof(BlogPostConsts.MaxContentLength))]
            public string Content { get; set; }

            [HiddenInput]
            public Guid? CoverImageMediaId { get; set; }

            public string ConcurrencyStamp { get; set; }
        }
    }
}
