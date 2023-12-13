using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.CmsKit.Contents;

[Serializable]
public class DefaultContentDto : IContent
{
    public List<ContentFragment> ContentFragments { get; set; }

    public bool AllowHtmlTags { get; set; } = true;

    public bool PreventXSS { get; set; } = true;

    [CanBeNull]
    public string ReferralLink { get; set; } = null;
}
