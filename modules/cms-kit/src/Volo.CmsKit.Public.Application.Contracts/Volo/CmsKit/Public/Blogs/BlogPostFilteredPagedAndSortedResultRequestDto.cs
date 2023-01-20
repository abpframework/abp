using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs;

[Serializable]
public class BlogPostFilteredPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
