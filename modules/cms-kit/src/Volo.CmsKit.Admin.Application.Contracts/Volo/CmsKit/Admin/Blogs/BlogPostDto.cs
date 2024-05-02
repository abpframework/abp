using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs;

[Serializable]
public class BlogPostDto : EntityDto<Guid>, IHasCreationTime, IHasModificationTime, IHasConcurrencyStamp
{
    public Guid BlogId { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string ShortDescription { get; set; }

    public string Content { get; set; }

    public Guid? CoverImageMediaId { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public string ConcurrencyStamp { get; set; }
    
    public BlogPostStatus Status { get; set; }
}
