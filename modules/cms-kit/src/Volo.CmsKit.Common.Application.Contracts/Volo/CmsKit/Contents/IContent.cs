using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public interface IContent 
{
    public List<ContentFragment> ContentFragments { get; set; }
}
