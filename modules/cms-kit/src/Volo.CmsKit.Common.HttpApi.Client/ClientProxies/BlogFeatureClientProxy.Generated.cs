using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Blogs.ClientProxies
{
    public partial class BlogFeatureClientProxy
    {
        public virtual async Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
        {
            return await RequestAsync<BlogFeatureDto>(nameof(GetOrDefaultAsync), blogId, featureName);
        }
 
    }
}
