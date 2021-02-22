using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogFeatureAdminAppService
    {
        Task SetAsync(Guid blogId, BlogFeatureInputDto dto);

        Task<List<BlogFeatureDto>> GetListAsync(Guid blogId);
    }
}
