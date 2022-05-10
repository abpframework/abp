using System;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs;

public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public Guid? BlogId { get; set; }

    public Guid? AuthorId { get; set; }

    public BlogPostStatus? Status { get; set; }
}