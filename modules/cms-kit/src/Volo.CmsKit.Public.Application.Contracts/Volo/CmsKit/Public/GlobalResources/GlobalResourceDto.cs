using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.GlobalResources;

[Serializable]
public class GlobalResourceDto : ExtensibleAuditedEntityDto
{
    public string Name { get; set; }

    public string Value { get; set; }
}