using System;

namespace Volo.CmsKit.Admin.GlobalResources;

[Serializable]
public class GlobalResourcesDto
{
    public string StyleContent { get; set; }
    
    public string ScriptContent { get; set; }
}