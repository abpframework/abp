using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public class DefaultContentDto : IContent
{
    public List<ContentFragment> ContentFragments { get; set; }
}
