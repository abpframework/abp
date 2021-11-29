using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogFeatureRepository : IBasicRepository<BlogFeature, Guid>
    {
        Task<List<BlogFeature>> GetListAsync(Guid blogId);

        Task<List<BlogFeature>> GetListAsync(Guid blogId, List<string> featureNames);

        Task<BlogFeature> FindAsync(Guid blogId, string featureName);
    }
}
