using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Public.Blogs;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs;

public class IndexModel : CmsKitPublicPageModelBase
{
    public const int PageSize = 12;

    [BindProperty(SupportsGet = true)]
    public string BlogSlug { get; set; }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public Guid? AuthorId { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? FilterOnFavorites { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid? TagId { get; set; }

    public PagedResultDto<BlogPostCommonDto> Blogs { get; protected set; }

    public PagerModel PagerModel => new PagerModel(Blogs.TotalCount, Blogs.Items.Count, CurrentPage, PageSize, Request.Path.ToString());

    public CmsUserDto SelectedAuthor { get; protected set; }

    public string FilteredTagName { get; protected set; }
    public BlogFeatureDto MarkedItemsFeature { get; private set; }

    protected IBlogPostPublicAppService BlogPostPublicAppService { get; }
    public IBlogFeatureAppService BlogFeatureAppService { get; }

    public IndexModel(
        IBlogPostPublicAppService blogPostPublicAppService,
        IBlogFeatureAppService blogFeatureAppService)
    {
        BlogPostPublicAppService = blogPostPublicAppService;
        BlogFeatureAppService = blogFeatureAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        Blogs = await BlogPostPublicAppService.GetListAsync(
            BlogSlug,
            new BlogPostGetListInput
            {
                SkipCount = PageSize * (CurrentPage - 1),
                MaxResultCount = PageSize,
                AuthorId = AuthorId,
                TagId = TagId,
                FilterOnFavorites = FilterOnFavorites
            });

        if (AuthorId != null)
        {
            SelectedAuthor = await BlogPostPublicAppService.GetAuthorHasBlogPostAsync(AuthorId.Value);
        }

        if (TagId is not null)
        {
            FilteredTagName = await BlogPostPublicAppService.GetTagNameAsync(TagId.Value);
        }

        if (GlobalFeatureManager.Instance.IsEnabled<MarkedItemsFeature>() &&
         Blogs.Items.Any())
        {
            var blogId = Blogs.Items.First().BlogId;
            MarkedItemsFeature = await BlogFeatureAppService.GetOrDefaultAsync(blogId, GlobalFeatures.MarkedItemsFeature.Name);
        }

        return Page();
    }
}
