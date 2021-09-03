using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Admin.Blogs;
using System.Collections.Generic;
using Volo.CmsKit.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies
{
    public partial class BlogFeatureAdminClientProxy
    {
        public virtual async Task<List<BlogFeatureDto>> GetListAsync(Guid blogId)
        {
            return await RequestAsync<List<BlogFeatureDto>>(nameof(GetListAsync), blogId);
        }
 
        public virtual async Task SetAsync(Guid blogId, BlogFeatureInputDto dto)
        {
            await RequestAsync(nameof(SetAsync), blogId, dto);
        }
 
    }
}
