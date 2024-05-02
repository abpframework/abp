using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs;

public interface IBlogFeatureRepository : IBasicRepository<BlogFeature, Guid>
{
    Task<List<BlogFeature>> GetListAsync(Guid blogId, CancellationToken cancellationToken = default);

    Task<List<BlogFeature>> GetListAsync(Guid blogId, List<string> featureNames, CancellationToken cancellationToken = default);

    Task<BlogFeature> FindAsync(Guid blogId, string featureName, CancellationToken cancellationToken = default);
}
