using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs;

public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public string BlogSlug { get; set; }

    public Guid? AuthorId { get; set; }
}