using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Admin.GlobalResources;

[Serializable]
public class GlobalResourcesUpdateDto : ExtensibleObject
{
    public string Style { get; set; }
    
    public string Script { get; set; }
}