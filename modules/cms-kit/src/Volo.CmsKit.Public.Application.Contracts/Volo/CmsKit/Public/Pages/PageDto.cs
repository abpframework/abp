using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Public.Pages;

[Serializable]
public class PageDto : EntityDto<Guid>
{
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }
    
    public List<ContentFragment> ContentFragments { get; set; }

    public string Script { get; set; }

    public string Style { get; set; }
}
