using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Admin.GlobalResources;

[Serializable]
public class GlobalResourcesDto : ExtensibleObject
{
    public string StyleContent { get; set; }
    
    public string ScriptContent { get; set; }
}