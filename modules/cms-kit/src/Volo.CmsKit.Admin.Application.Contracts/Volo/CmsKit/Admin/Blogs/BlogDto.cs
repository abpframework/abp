using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Admin.Blogs;

[Serializable]
public class BlogDto : EntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public string Slug { get; set; }
    public string ConcurrencyStamp { get; set; }
}
