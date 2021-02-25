using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogFeatureManager : IDomainService
    {
        Task<List<BlogFeature>> GetListAsync(Guid blogId);

        Task SetAsync(Guid blogId, string featureName, bool isEnabled);
    }
}
