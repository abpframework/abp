using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.CmsKit.Public.Blogs
{
    public interface IBlogFeaturePublicAppService
    {
        Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName);
    }
}
