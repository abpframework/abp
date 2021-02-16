using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogFeatureAdminAppService
    {
        Task SetAsync(Guid blogId, BlogFeatureDto dto);

        Task<List<BlogFeatureDto>> GetListAsync(Guid blogId);
    }
}
