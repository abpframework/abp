using System.Collections.Generic;
using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Contents;

[Serializable]
public class PageDto : EntityDto<Guid>, IContent
{
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public List<ContentFragment> ContentFragments { get; set; }

    public string Script { get; set; }

    public string Style { get; set; }
}