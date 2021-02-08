using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogAdminAppService : ICrudAppService<BlogDto, Guid, BlogGetListInput>
    {
    }
}
