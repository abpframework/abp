using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Pages.Admin.Blogs
{
    public class CreateModel : AbpPageModel
    {
        private readonly IBlogAppService _blogAppService;

        [BindProperty]
        public BlogCreateModalView Blog { get; set; } = new BlogCreateModalView();

        public CreateModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public void OnGet()
        {

        }

        public async void OnPostAsync()
        {
            var language = ObjectMapper.Map<BlogCreateModalView, CreateBlogDto>(Blog);

            await _blogAppService.Create(language);
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

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Facebook { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Twitter { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Instagram { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Github { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string StackOverflow { get; set; }

        }
    }
}