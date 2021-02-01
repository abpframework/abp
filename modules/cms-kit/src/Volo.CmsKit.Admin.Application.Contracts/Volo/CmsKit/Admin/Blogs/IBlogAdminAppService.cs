using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Admin.Application.Contracts.Volo.CmsKit.Admin.Blogs;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogAdminAppService : ICrudAppService<BlogDto, Guid>
    {
        Task<List<BlogLookupDto>> GetLookupAsync();
    }
}
