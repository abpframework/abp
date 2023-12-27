using System.Collections.Generic;

namespace Volo.CmsKit.Contents;

public interface IContent 
{
    List<ContentFragment> ContentFragments { get; set; }

    bool AllowHtmlTags { get; set; }
    
    bool PreventXSS { get; set; }

    string ReferralLink { get; set; }
}
