using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs;

public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public Guid? BlogId { get; set; }
}
