using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Admin.Blogs
{
    public interface IBlogManagementAppService : IApplicationService
    {
        Task<ListResultDto<BlogDto>> GetListAsync();

        Task<BlogDto> GetAsync(Guid id);

        Task<BlogDto> CreateAsync(CreateBlogDto input);

        Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input);

        Task DeleteAsync(Guid id);

        Task ClearCacheAsync(Guid id);
    }
}
