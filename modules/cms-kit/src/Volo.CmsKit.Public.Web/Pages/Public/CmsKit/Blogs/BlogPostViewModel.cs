using System;
using System.Collections.Generic;
using AutoMapper;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs;

[AutoMap(typeof(BlogPostCommonDto), ReverseMap = true)]
public class BlogPostViewModel : AuditedEntityDto<Guid>
{
    public Guid BlogId { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public string ShortDescription { get; set; }

    public string Content { get; set; }

    public List<ContentFragment> ContentFragments { get; set; }

    public Guid? CoverImageMediaId { get; set; }

    public CmsUserDto Author { get; set; }
}
