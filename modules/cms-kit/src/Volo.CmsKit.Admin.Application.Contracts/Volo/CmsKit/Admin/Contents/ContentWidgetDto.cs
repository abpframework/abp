using System;
using System.Collections.Generic;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Admin.Contents;

[Serializable]
public class ContentWidgetDto
{
    public string Key { get; set; }
    public List<PropertyDto> Properties { get; set; } = new();
}
