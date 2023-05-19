using System;
using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

[Serializable]
public class DefaultContentDto : IContent
{
    public List<ContentFragment> ContentFragments { get; set; }
}
