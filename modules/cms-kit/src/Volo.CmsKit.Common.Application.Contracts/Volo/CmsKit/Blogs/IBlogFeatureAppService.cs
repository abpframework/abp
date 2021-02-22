using System;
using System.Threading.Tasks;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogFeatureAppService
    {
        Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName);
    }
}
