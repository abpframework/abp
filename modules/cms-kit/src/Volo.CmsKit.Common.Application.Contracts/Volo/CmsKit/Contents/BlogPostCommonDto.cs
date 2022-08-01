using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Contents;

[Serializable]
public class BlogPostCommonDto : AuditedEntityDto<Guid>, IContent
{
    public Guid BlogId { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string ShortDescription { get; set; }

    public List<ContentFragment> ContentFragments { get; set; }

    public string Content { get; set; }

    public Guid? CoverImageMediaId { get; set; }

    public CmsUserDto Author { get; set; }
}