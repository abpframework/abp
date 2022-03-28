using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs;

public class BlogPostFilteredPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
