using System;

namespace Volo.CmsKit.Public.GlobalResources;

[Serializable]
public class GlobalResourceDto
{
    public string Name { get; set; }

    public string Value { get; set; }

    public DateTime? LastModificationTime { get; set; }
}