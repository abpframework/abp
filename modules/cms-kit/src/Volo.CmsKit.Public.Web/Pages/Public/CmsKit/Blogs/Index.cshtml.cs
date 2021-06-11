using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs
{
    public class IndexModel : CmsKitPublicPageModelBase
    {
        public const int PageSize = 12;

        [BindProperty(SupportsGet = true)]
        public string BlogSlug { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public PagedResultDto<BlogPostPublicDto> Blogs { get; private set; }

        public PagerModel PagerModel => new PagerModel(Blogs.TotalCount, Blogs.Items.Count, CurrentPage, PageSize, Request.Path.ToString());

        protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

        public IndexModel(IBlogPostPublicAppService blogPostPublicAppService)
        {
            BlogPostPublicAppService = blogPostPublicAppService;
        }

        public async Task OnGetAsync()
        {
            Blogs = await BlogPostPublicAppService.GetListAsync(
                BlogSlug, 
                new PagedAndSortedResultRequestDto
                {
                    SkipCount = PageSize * (CurrentPage - 1),
                    MaxResultCount = PageSize
                });
        }
    }
}
