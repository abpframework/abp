using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Blogs;

public interface IDefaultBlogFeatureProvider
{
    Task<List<BlogFeature>> GetDefaultFeaturesAsync(Guid blogId);
}
