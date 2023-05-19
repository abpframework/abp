using System;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Admin.Contents;

[Serializable]
public class ContentWidgetDto
{
    public string Key { get; set; }
    
    public WidgetDetailDto Details { get; set; }
}