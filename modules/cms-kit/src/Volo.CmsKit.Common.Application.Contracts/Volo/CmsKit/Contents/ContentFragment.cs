using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Contents;

[Serializable]
public class ContentFragment : ExtensibleObject
{
    public string Type { get; set; }
}
