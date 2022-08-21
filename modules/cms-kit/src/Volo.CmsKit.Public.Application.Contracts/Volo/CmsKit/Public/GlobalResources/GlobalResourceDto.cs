using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.GlobalResources;

[Serializable]
public class GlobalResourceDto : AuditedEntityDto
{
    public string Name { get; set; }

    public string Value { get; set; }
}