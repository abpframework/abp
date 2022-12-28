using System;

namespace Volo.CmsKit.Admin.GlobalResources;

[Serializable]
public class GlobalResourcesUpdateDto
{
    public string Style { get; set; }
    
    public string Script { get; set; }
}