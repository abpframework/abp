using System;
using System.Collections.Generic;

namespace Volo.CmsKit.Public.Blogs;

[Serializable]
public class GetBlogFeatureInput
{
    public List<string> FeatureNames { get; set; }
}
