using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs;

public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public Guid? AuthorId { get; set; }
}