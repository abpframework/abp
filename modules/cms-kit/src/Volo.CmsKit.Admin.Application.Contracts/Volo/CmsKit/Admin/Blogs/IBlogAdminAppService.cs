using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs;

public interface IBlogAdminAppService : ICrudAppService<BlogDto, Guid, BlogGetListInput, CreateBlogDto, UpdateBlogDto>
{
    Task<ListResultDto<BlogDto>> GetAllListAsync();
    
    Task MoveAllBlogPostsAsync(Guid blogId, Guid? assignToBlogId = null);
}
