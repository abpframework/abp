using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogFeatureAdminAppService : IApplicationService
    {
        Task SetAsync(Guid blogId, BlogFeatureInputDto dto);

        Task<List<BlogFeatureDto>> GetListAsync(Guid blogId);
    }
}
