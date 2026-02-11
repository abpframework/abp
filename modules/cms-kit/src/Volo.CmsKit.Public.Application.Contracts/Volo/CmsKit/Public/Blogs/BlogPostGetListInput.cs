using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs;

public class BlogPostGetListInput : PagedAndSortedResultRequestDto
{
    public Guid? AuthorId { get; set; } 
    
    public Guid? TagId { get; set; }
    public bool? FilterOnFavorites { get; set; }
}