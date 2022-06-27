using System;
using System.Collections.Generic;

namespace Volo.CmsKit.Admin.Contents;

[Serializable]
public class ContentWidgetDto
{
    public string Key { get; set; }
    public List<string> Properties { get; set; } = new();
}
