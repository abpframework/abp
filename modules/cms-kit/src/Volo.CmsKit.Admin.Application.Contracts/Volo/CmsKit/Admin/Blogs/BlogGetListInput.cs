using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Blogs;

[Serializable]
public class BlogGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
