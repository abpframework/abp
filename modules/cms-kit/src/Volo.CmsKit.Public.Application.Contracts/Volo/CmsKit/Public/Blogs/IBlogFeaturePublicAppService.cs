using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.CmsKit.Public.Blogs
{
    public interface IBlogFeaturePublicAppService
    {
        Task<BlogFeatureDto> GetAsync(Guid blogId, string featureName);

        Task<List<BlogFeatureDto>> GetManyAsync(Guid blogId, GetBlogFeatureInput input);
    }
}
