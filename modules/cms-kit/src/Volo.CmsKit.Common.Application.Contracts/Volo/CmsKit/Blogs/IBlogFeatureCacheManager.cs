using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogFeatureCacheManager
    {
        Task<BlogFeatureDto> AddOrGetAsync(Guid blogId, string featureName, Func<Task<BlogFeatureDto>> factory);
        Task ClearAsync(Guid blogId, string featureName);
    }
}
