using System;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogAdminAppService : ICrudAppService<BlogDto, Guid, BlogGetListInput, CreateBlogDto, UpdateBlogDto>
    {
    }
}
