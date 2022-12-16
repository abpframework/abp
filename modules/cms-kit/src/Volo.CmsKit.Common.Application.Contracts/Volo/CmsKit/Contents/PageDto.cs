using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Volo.CmsKit.Contents;

[Serializable]
public class PageDto : EntityDto<Guid>, IHasEntityVersion
{
    public string Title { get; set; }

    public string Slug { get; set; }

    public string Content { get; set; }

    public string Script { get; set; }

    public string Style { get; set; }

    public int EntityVersion { get; set; }
}