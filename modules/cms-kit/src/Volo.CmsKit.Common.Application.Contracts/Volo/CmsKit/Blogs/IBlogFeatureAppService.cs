using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogFeatureAppService : IApplicationService
    {
        Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName);
    }
}
